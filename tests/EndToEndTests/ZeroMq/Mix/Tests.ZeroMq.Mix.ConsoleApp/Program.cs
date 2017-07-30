using System;
using System.Threading;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.Mix.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factoryAsync = new ZeroMqMessageQueueFactoryAsync();
            var reqQueue = factoryAsync.CreateOutboundQueue("mix-customer", MessagePattern.RequestResponse);

            for (var i = 0; i < 5; i++)
            {
                reqQueue.Send(new Message
                {
                    Body = new CreateCustomerRequest(i, $"ahmad {i}")
                    //Body = new CreateCustomerRequest(1, $"ahmad {1}")
                });

                var resQueue = reqQueue.GetResponseQueue();
                resQueue.Received(Process);
            }

            //Process(null);

            Thread.Sleep(50000);
        }

        private static void Process(Message message)
        {
            Console.WriteLine(message.BodyAs<string>());

            var factory = new ZeroMqMessageQueueFactory();
            var subQueue = factory.CreateInboundQueue(
                new MessageQueueConfig("mix-publish", MessagePattern.PublishSubscribe)
                {
                    SubscribeKey = message.BodyAs<string>()
                    //SubscribeKey = "0d7b8247-d74a-4060-b0bf-a006db9d182c"
                });


            Console.WriteLine("Subscribed...");
            subQueue.Received(ProcessReceive);
        }

        private static void ProcessReceive(Message message)
        {
            message.BodyAs<CustomerCreatedResponse>().ShowOnConsole();
        }
    }
}