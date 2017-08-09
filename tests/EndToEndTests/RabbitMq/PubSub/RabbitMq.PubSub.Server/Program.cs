using System;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.RabbitMq;

namespace RabbitMq.PubSub.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopServer();

            var factory = new RabbitMqMessageQueueFactory();
            var server = factory.CreateInboundQueue("log", MessagePattern.PublishSubscribe);
            Common.Show("Waiting ...");

            server.Received(message => { Common.Show(message.BodyAs<string>()); });


            Common.Separator();
            Console.ReadKey();
        }
    }
}