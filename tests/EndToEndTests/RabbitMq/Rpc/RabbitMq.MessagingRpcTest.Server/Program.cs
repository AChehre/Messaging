using System;
using System.Threading;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.RabbitMq;

namespace RabbitMq.MessagingRpcTest.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopServer();

            var client = new RabbitMqMessageQueueRpcServer(
                new RabbitMqConfig("localhost", "guest", "guest", null) {CreateExchange = false, CreateQueue = false});
            client.InitializeInbound("", MessagePattern.FireAndForget);


            client.Received(message => OnReceived(message, client));


            Common.ScreenEnd();

            Console.ReadKey();
        }


        private static void OnReceived(Message message, IMessageQueue messageQueue)
        {
            var body = message.BodyAs<string>();
            Common.Show(body);

            Thread.Sleep(2000);

            messageQueue.Send(new Message
            {
                Body = body + body,
                ResponseAddress = message.ResponseAddress,
                Properties = message.Properties,
                ResponseKey = message.ResponseKey
            });

            Common.Show($"Send for {body}");
        }
    }
}