using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public abstract class MessageQueueFactory : IMessageQueueFactory
    {
        protected static readonly IDictionary<string, IMessageQueue> _queues;

        static MessageQueueFactory()
        {
            if (_queues == null)
                _queues = new ConcurrentDictionary<string, IMessageQueue>();
        }

        //protected  readonly IDictionary<string, IMessageQueue> _queues;

        //protected MessageQueueFactory()
        //{
        //        _queues = new ConcurrentDictionary<string, IMessageQueue>();
        //}

        public IMessageQueue CreateInboundQueue(string name, MessagePattern pattern)
        {
            var key =
                $"{Direction.Inbound}:{name}:{pattern}";
            if (_queues.ContainsKey(key))
                return _queues[key];

            var que = CreateMessageQueue(Direction.Inbound);

            que.InitializeInbound(name, pattern);

            _queues[key] = que;
            return _queues[key];
        }

        public IMessageQueue CreateInboundQueue(MessageQueueConfig config)
        {
            var key =
                $"{Direction.Inbound}:{config.Name}:{config.MessagePattern}";
            if (_queues.ContainsKey(key))
            {
                var q = _queues[key];
                q.InitializeInbound(config);
                return q;
            }
            var que = CreateMessageQueue(Direction.Inbound);

            que.InitializeInbound(config);

            _queues[key] = que;
            return _queues[key];
        }

        public IMessageQueue CreateOutboundQueue(string name, MessagePattern pattern)
        {
            var key =
                $"{Direction.OutBound}:{name}:{pattern}";
            if (_queues.ContainsKey(key))
                return _queues[key];

            var que = CreateMessageQueue(Direction.OutBound);
            que.InitializeOutbound(name, pattern);

            _queues[key] = que;
            return _queues[key];
        }

        public abstract IMessageQueue CreateMessageQueue(Direction direction);
    }
}