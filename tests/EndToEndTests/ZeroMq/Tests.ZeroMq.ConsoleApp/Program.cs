﻿using System;
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

            Listen("Customer", MessagePattern.RequestResponse);

            Console.WriteLine("Listening ended.");
            Console.ReadKey();
        }

        private static void Listen(string name, MessagePattern pattern)
        {
            var factory = new ZeroMqMessageQueueFactory();
            var queue = factory.CreateInboundQueue(name, pattern);
            queue.Listen(message => Customer(queue, message));
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
            var customerService = new CustomerService();
            if (message.MessageType == typeof(CreateCustomerRequest).Name)
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    customerService.CreateCustomer(queue, message);
                }).Start();
            else if (message.MessageType == typeof(DeleteCustomerRequest).Name)
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    customerService.DeleteCustomer(queue, message);
                }).Start();
        }
    }
}