using System;
using System.Threading;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Start listening.");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Listen("CreateCustomer", MessagePattern.RequestResponse);
            }).Start();


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Listen("DeleteCustomer", MessagePattern.RequestResponse);
            }).Start();


            //Listen("Customer", MessagePattern.RequestResponse);

      
        }

        private static void Listen(string name, MessagePattern pattern)
        {
            var factory = new ZeroMqMessageQueueFactoryAsync();
            var queue = factory.CreateInboundQueue(name, pattern);
            queue.Listen(message => CustomerInThreads(queue, message));
        }

        private static void Customer(IMessageQueue queue, Message message)
        {
            var customerService = new CustomerService();
            if (message.MessageType == typeof(CreateCustomerRequest).Name)
                customerService.CreateCustomer(queue, message);
            else if (message.MessageType == typeof(DeleteCustomerRequest).Name)
                customerService.DeleteCustomer(queue, message);
        }

        private static void CustomerInThreads(IMessageQueue queue, Message message)
        {
           
            if (message.MessageType == typeof(CreateCustomerRequest).Name)
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    var customerService = new CustomerService();
                    customerService.CreateCustomer(queue, message);
                }).Start();
            else if (message.MessageType == typeof(DeleteCustomerRequest).Name)
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    var customerService = new CustomerService();
                    customerService.DeleteCustomer(queue, message);
                }).Start();
        }
    }
}