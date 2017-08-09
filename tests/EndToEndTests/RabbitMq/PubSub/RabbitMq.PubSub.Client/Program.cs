using System;
using System.Runtime.InteropServices.ComTypes;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.RabbitMq;

namespace RabbitMq.PubSub.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Common.ScreenTopClient();

           var factory = new RabbitMqMessageQueueFactory();
            var client = factory.CreateOutboundQueue("log", MessagePattern.PublishSubscribe);
            Common.Show("Sending ...");
                client.Send(new Message()
                {
                    Body = "Hi ..."
                });

            Common.Separator();
            Console.ReadKey();

        }
    }
}