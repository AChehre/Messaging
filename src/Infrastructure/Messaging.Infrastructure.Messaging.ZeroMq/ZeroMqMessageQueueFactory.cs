namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueFactory : MessageQueueFactory
    {
        public override ISyncMessageQueue CreateMessageQueue()
        {
            return new ZeroMqMessageQueue();
        }

        public override IASyncMessageQueue CreateMessageQueueAsync()
        {
            return new ZeroMqMessageQueueAsync();
        }
    }
}