namespace Messaging.Infrastructure.Messaging
{
    public class MessageQueueConfig
    {
        public MessageQueueConfig()
        {
            
        }
        public MessageQueueConfig(string name, MessagePattern pattern)
        {
            MessageQueueName = name;
            MessagePattern = pattern;
        }
        public string SubscribeKey { get; set; }
        public string MessageQueueName { get; set; }
        public MessagePattern MessagePattern { get; set; }


        public RabbitMqMessageQueueConfig RabbitMq { get; set; }
        
    }


    public class RabbitMqMessageQueueConfig
    {
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
    }
}