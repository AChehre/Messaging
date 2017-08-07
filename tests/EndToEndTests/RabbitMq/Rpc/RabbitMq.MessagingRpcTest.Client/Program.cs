using System;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.RabbitMq;

namespace RabbitMq.MessagingRpcTest.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopClient();

            var factory = new RabbitMqMessageQueueFactory();

            var client = factory.CreateOutboundQueue("", MessagePattern.FireAndForget);

            for (var i = 0; i < 5; i++)
            {
                client.Send(new Message {Body = $"{i}"}, "rpc_queue");
                client.Received(OnReceived);
            }


            Common.ScreenEnd();

            Console.ReadKey();
        }

        private static void OnReceived(Message message)
        {
            Common.Show(message.BodyAs<string>());
        }
    }
}