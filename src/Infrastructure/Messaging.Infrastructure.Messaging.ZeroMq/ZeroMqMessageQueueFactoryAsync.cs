namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueFactoryAsync : MessageQueueFactory
    {
        public override IMessageQueue CreateMessageQueue()
        {
            return new ZeroMqMessageQueueAsync();
        }
    }
}