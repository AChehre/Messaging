namespace Messaging.Infrastructure.Messaging
{
    public abstract class MessageQueueConfig
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public MessagePattern MessagePattern { get; set; }
      
    }
}