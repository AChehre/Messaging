namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueFactory : MessageQueueFactory
    {
        public override IMessageQueue CreateMessageQueue()
        {
            return new ZeroMqMessageQueue();
        }
    }
}