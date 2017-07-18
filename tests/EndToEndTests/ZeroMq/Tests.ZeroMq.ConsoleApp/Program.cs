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
            Console.WriteLine("Start listening.");

            Listen("CreateCustomer", MessagePattern.RequestResponse);

            Console.WriteLine("Listening ended.");
            Console.ReadKey();
            
        }

        private static void Listen(string name, MessagePattern pattern)
        {
            var factory = new ZeroMqMessageQueueFactory();

            var queue = factory.CreateInboundQueue(name, pattern);
            queue.Listen(q =>
            {
                (new CustomerService()).CreateCustomer(queue, q);
            });
        }
    }
}