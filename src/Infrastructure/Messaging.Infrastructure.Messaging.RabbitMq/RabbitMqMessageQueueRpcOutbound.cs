using System;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueRpcOutbound : BaseRabbitMqMessageQueueRpc, IMessageQueue
    {
#pragma warning disable 0618
        private QueueingBasicConsumer _consumer;
#pragma warning restore 0618
        private string _correlationId;
        private string _replyQueueName;


        public RabbitMqMessageQueueRpcOutbound(RabbitMqConfig rabbitMqConfig) : base(rabbitMqConfig)
        {
        }


        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            Config = new MessageQueueConfig(name, pattern);
            Channel = CreateChannel();


            _replyQueueName = Channel.QueueDeclare().QueueName;
#pragma warning disable 0618
            _consumer = new QueueingBasicConsumer(Channel);
#pragma warning restore 0618
            Channel.BasicConsume(_replyQueueName,
                true,
                _consumer);
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            var config = new MessageQueueConfig(name, pattern);
            InitializeInbound(config);
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
        }

        public void Send(Message message)
        {
        }

        public void Send(Message message, string key)
        {
            _correlationId = Guid.NewGuid().ToString();
            var props = Channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = _correlationId;

            var messageBytes = Encoding.UTF8.GetBytes(message.ToJson());
            Channel.BasicPublish(Config.Name, key,
                props,
                messageBytes);
        }

        public void Received(Action<Message> onMessageReceived)
        {
            var ea = _consumer.Queue.Dequeue();
            if (ea.BasicProperties.CorrelationId != _correlationId)
                return;
            var result = Encoding.UTF8.GetString(ea.Body);
            onMessageReceived.Invoke(result.DeserializeFromJson<Message>());
        }
    }
}