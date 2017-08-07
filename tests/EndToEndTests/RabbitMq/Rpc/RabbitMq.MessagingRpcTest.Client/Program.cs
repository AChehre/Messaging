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

            var client = new RabbitMqMessageQueueRpcClient(
                new RabbitMqConfig("localhost", "guest", "guest", null) {CreateExchange = false, CreateQueue = false});
            client.InitializeOutbound("", MessagePattern.FireAndForget);

            for (int i = 0; i < 5; i++)
            {
                client.Send(new Message { Body = $"{i}" }, "rpc_queue");
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