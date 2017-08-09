using System;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueuePubSub : BaseRabbitMqMessageQueueRpc, IMessageQueue
    {
#pragma warning disable 0618
        private QueueingBasicConsumer _consumer;
#pragma warning restore 0618

        public RabbitMqMessageQueuePubSub(RabbitMqConfig rabbitMqConfig) : base(rabbitMqConfig)
        {
        }

        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
#pragma warning disable 0618
            _consumer = new QueueingBasicConsumer(Channel);
#pragma warning restore 0618
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            throw new NotImplementedException();
        }

        public void Send(Message message)
        {
            throw new NotImplementedException();
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            var ea = _consumer.Queue.Dequeue();

            var result = Encoding.UTF8.GetString(ea.Body);
            onMessageReceived.Invoke(result.DeserializeFromJson<Message>());
        }
    }
}