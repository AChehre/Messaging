using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public abstract class MessageQueueFactory<TMessageQueueConfig> : IMessageQueueFactory<TMessageQueueConfig>
        where TMessageQueueConfig : MessageQueueConfig
    {
        protected readonly IDictionary<string, IMessageQueue<TMessageQueueConfig>> _queues;

        protected MessageQueueFactory()
        {
            _queues = new ConcurrentDictionary<string, IMessageQueue<TMessageQueueConfig>>();
        }

        public IMessageQueue<TMessageQueueConfig> CreateInboundQueue(TMessageQueueConfig messageQueueConfig)
        {
            var key =
                $"{Direction.Inbound}:{messageQueueConfig.MessageQueueName}:{messageQueueConfig.MessagePattern}";
            if (_queues.ContainsKey(key))
                return _queues[key];

            var que = CreateMessageQueue();
            que.InitializeInbound(messageQueueConfig);
            _queues[key] = que;
            return _queues[key];
        }

        public IMessageQueue<TMessageQueueConfig> CreateOutnboundQueue(TMessageQueueConfig messageQueueConfig)
        {
            var key =
                $"{Direction.OutBound}:{messageQueueConfig.MessageQueueName}:{messageQueueConfig.MessagePattern}";
            if (_queues.ContainsKey(key))
                return _queues[key];

            var que = CreateMessageQueue();
            que.InitializeOutbound(messageQueueConfig);
            _queues[key] = que;
            return _queues[key];
        }


        public abstract IMessageQueue<TMessageQueueConfig> CreateMessageQueue();
    }
}