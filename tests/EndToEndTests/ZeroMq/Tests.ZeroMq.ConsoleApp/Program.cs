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

            Listen("Customer", MessagePattern.RequestResponse);

            Console.WriteLine("Listening ended.");
            Console.ReadKey();
            
        }

        private static void Listen(string name, MessagePattern pattern)
        {
            var customerService = new  CustomerService();
            var factory = new ZeroMqMessageQueueFactory();

            var queue = factory.CreateInboundQueue(name, pattern);
            queue.Listen(message =>Customer(queue, message));
        }

        private static void Customer(IMessageQueue queue, Message message)
        {
            var customerService = new CustomerService();
            if (message.MessageType == typeof(CreateCustomerRequest).Name)
            {
                customerService.CreateCustomer(queue, message);
            }
            else
            if (message.MessageType == typeof(DeleteCustomerRequest).Name)
            {
                customerService.DeleteCustomer(queue, message);
            }

        }
    }
}