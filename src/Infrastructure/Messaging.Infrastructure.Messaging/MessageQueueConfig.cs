namespace Messaging.Infrastructure.Messaging
{
    public abstract class MessageQueueConfig
    {
        public string MessageQueueName { get; set; }
        public MessagePattern MessagePattern { get; set; }
    }
}