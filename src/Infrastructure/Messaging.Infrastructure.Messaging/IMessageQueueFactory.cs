namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueueFactory
    {
        ISyncMessageQueue CreateInboundQueue(string name, MessagePattern pattern);
        ISyncMessageQueue CreateOutboundQueue(string name, MessagePattern pattern);
        IASyncMessageQueue CreateInboundQueueAsync(string name, MessagePattern pattern);
        IASyncMessageQueue CreateOutboundQueueAsync(string name, MessagePattern pattern);
    }
}