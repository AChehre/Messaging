namespace Messaging.Infrastructure.Messaging
{
    public class MessageQueueConfig
    {
        public MessageQueueConfig()
        {
            
        }
        public MessageQueueConfig(string name, MessagePattern pattern)
        {
            Name = name;
            MessagePattern = pattern;
        }
        public string SubscribeKey { get; set; }
        public string Name { get; set; }
        public MessagePattern MessagePattern { get; set; }

    }


}