namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueueFactory
    {
        IMessageQueue CreateInboundQueue(string name, MessagePattern pattern);
        IMessageQueue CreateOutboundQueue(string name, MessagePattern pattern);
    }
}