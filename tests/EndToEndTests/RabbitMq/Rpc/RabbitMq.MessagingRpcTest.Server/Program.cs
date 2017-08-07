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

            var factory = new RabbitMqMessageQueueFactory();
            var server = factory.CreateInboundQueue("rpc_queue", MessagePattern.FireAndForget);


            Common.Show("Waiting ...");

            server.Received(message => OnReceived(message, server));


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