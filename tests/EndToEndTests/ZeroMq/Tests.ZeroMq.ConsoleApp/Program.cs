using System;
using System.Diagnostics;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        private static void Listen(string name, MessagePattern pattern)
        {
            var factory = new ZeroMqMessageQueueFactory();
            var config = new ZeroMqMessageQueueConfig
            {
                MessageQueueName = "CreateCustomer",
                MessagePattern = MessagePattern.RequestResponse
            };
            var queue = factory.CreateInboundQueue(config);
            //queue.Listen(q =>
            //{
            //    (new CustomerService()).CreateCustomer(queue, q);
            //});
        }
    }
}