namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueueFactory<TMessageQueueConfig> where TMessageQueueConfig : MessageQueueConfig
    {
        IMessageQueue<TMessageQueueConfig> CreateInboundQueue(TMessageQueueConfig messageQueueConfig);
        IMessageQueue<TMessageQueueConfig> CreateOutnboundQueue(TMessageQueueConfig messageQueueConfig);
    }
}