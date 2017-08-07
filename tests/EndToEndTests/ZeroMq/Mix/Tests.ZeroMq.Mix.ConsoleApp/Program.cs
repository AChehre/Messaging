using System;
using System.Threading;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.Mix.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTop("Client");

            var factoryAsync = new ZeroMqMessageQueueFactoryAsync();
            var reqQueue = factoryAsync.CreateOutboundQueue("mix-customer", MessagePattern.RequestResponse);

            var startNumber = 0;

            if (args != null && args.Length > 0)
                startNumber = Convert.ToInt32(args[0]);


            for (var i = 1 + startNumber; i < 6 + startNumber; i++)
            {
                Common.Show(new string('-', 20));

                reqQueue.Send(new Message
                {
                    Body = new CreateCustomerRequest(i, $"ahmad {i}")
                });

                var resQueue = reqQueue.GetResponseQueue();
                resQueue.Received(Process);
            }


            Thread.Sleep(50000);
        }

        private static void Process(Message message)
        {
            var factory = new ZeroMqMessageQueueFactory();
            var subQueue = factory.CreateInboundQueue(
                new MessageQueueConfig("mix-publish", MessagePattern.PublishSubscribe)
                {
                    SubscribeKey = message.BodyAs<string>()
                });


            Common.Show($"Subscribed on {message.BodyAs<string>()}");
            subQueue.Received(ProcessReceive);
        }

        private static void ProcessReceive(Message message)
        {
            message.BodyAs<CustomerCreatedResponse>().ShowOnConsole();
        }
    }
}