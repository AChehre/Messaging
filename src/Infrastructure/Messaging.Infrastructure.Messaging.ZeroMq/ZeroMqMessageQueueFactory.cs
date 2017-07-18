namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueFactory : MessageQueueFactory<ZeroMqMessageQueueConfig>
    {
        public override IMessageQueue<ZeroMqMessageQueueConfig> CreateMessageQueue()
        {
            return new ZeroMqMessageQueue();
        }
    }
}