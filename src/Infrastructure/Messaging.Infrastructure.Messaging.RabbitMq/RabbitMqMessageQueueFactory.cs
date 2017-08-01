namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueFactory : MessageQueueFactory
    {
        public override IMessageQueue CreateMessageQueue()
        {
            return new RabbitMqMessageQueue();
        }
    }
}