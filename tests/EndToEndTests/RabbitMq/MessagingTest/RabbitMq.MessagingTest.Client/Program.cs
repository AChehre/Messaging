using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.RabbitMq;

namespace RabbitMq.MessagingTest.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopClient();

            var factory = new RabbitMqMessageQueueFactory();
            var client = factory.CreateOutboundQueue("fanoutexchange", MessagePattern.FireAndForget);

            client.Send(new Message
            {
                Body = $"Hi ..."
            });
        }
    }
}