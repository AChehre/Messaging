namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueueFactory
    {
        IMessageQueue CreateInboundQueue(string name, MessagePattern pattern);
        IMessageQueue CreateOutnboundQueue(string name, MessagePattern pattern);
    }
}