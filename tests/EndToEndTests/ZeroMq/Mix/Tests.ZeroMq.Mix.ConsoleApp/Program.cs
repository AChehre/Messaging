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
            ScreenTop("Client");

            var factoryAsync = new ZeroMqMessageQueueFactoryAsync();
            var reqQueue = factoryAsync.CreateOutboundQueue("mix-customer", MessagePattern.RequestResponse);

            for (var i = 0; i < 5; i++)
            {
                Show(new string('-', 20));

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


            Show($"Subscribed on {message.BodyAs<string>()}");
            subQueue.Received(ProcessReceive);
        }

        private static void ProcessReceive(Message message)
        {
            message.BodyAs<CustomerCreatedResponse>().ShowOnConsole();
        }

        private static void Show(string message)
        {
            Console.WriteLine($"{message}\n");

        }

        private static void ScreenTop(string title)
        {
            var dashes = new string('-', title.Length + 20);

            Console.WriteLine(dashes);
            Console.WriteLine($"|{new string(' ', 9)}{title}{new string(' ', 9)}|");
            Console.WriteLine(dashes);
        }
    }
}