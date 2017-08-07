using System;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueFactory : IMessageQueueFactory
    {
        public IMessageQueue CreateInboundQueue(string name, MessagePattern pattern)
        {
            var que = CreateMessageQueue(Direction.Inbound);

            que.InitializeInbound(name, pattern);
            return que;
        }

        public IMessageQueue CreateInboundQueue(MessageQueueConfig config)
        {
            var que = CreateMessageQueue(Direction.Inbound);

            que.InitializeInbound(config.Name, config.MessagePattern);
            return que;
        }

        public IMessageQueue CreateOutboundQueue(string name, MessagePattern pattern)
        {
            var que = CreateMessageQueue(Direction.OutBound);
            que.InitializeOutbound(name, pattern);
            return que;
        }


        public IMessageQueue CreateMessageQueue(Direction direction)
        {
            var bindings = new RabbitMqBinding
            {
                new RabbitMqBindingItem("fanout-exchange", "fanout-exchange-queue", "")
            };

            var rabbitMqConfig = new RabbitMqConfig("localhost", "guest", "guest", bindings)
            {
                CreateExchange = true
            };

            switch (direction)
            {
                case Direction.Inbound:
                    return new RabbitMqMessageQueueRpcInbound(rabbitMqConfig);
                case Direction.OutBound:
                    return new RabbitMqMessageQueueRpcOutbound(rabbitMqConfig);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}