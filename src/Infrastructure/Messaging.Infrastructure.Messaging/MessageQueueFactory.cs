using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public abstract class MessageQueueFactory : IMessageQueueFactory
    {
        protected readonly IDictionary<string, IMessageQueue> _queues;

        protected MessageQueueFactory()
        {
            _queues = new ConcurrentDictionary<string, IMessageQueue>();
        }

        public IMessageQueue CreateInboundQueue(string name, MessagePattern pattern)
        {
            var key =
                $"{Direction.Inbound}:{name}:{pattern}";
            if (_queues.ContainsKey(key))
                return _queues[key];

            var que = CreateMessageQueue();
            que.InitializeInbound(name, pattern);
            _queues[key] = que;
            return _queues[key];
        }

        public IMessageQueue CreateOutnboundQueue(string name, MessagePattern pattern)
        {
            var key =
                $"{Direction.OutBound}:{name}:{pattern}";
            if (_queues.ContainsKey(key))
                return _queues[key];

            var que = CreateMessageQueue();
            que.InitializeOutbound(name, pattern);
            _queues[key] = que;
            return _queues[key];
        }


        public abstract IMessageQueue CreateMessageQueue();
    }
}