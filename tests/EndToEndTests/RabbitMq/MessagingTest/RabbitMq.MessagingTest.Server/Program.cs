using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.RabbitMq;

namespace RabbitMq.MessagingTest.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopServer();

            var factory = new RabbitMqMessageQueueFactory();
            var client = factory.CreateInboundQueue("fanoutexchangequeue", MessagePattern.FireAndForget);

            client.Received(message => { Common.Show(message.BodyAs<string>()); });

            Common.Show("End...");
        }
    }
}