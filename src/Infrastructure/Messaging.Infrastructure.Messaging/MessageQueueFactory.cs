using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public abstract class MessageQueueFactory : IMessageQueueFactory
    {
        protected static readonly IDictionary<string, ISyncMessageQueue> _queues;
        protected static readonly IDictionary<string, IASyncMessageQueue> _asyncQueues;

        static MessageQueueFactory()
        {
            if (_queues == null)
                _queues = new ConcurrentDictionary<string, ISyncMessageQueue>();
        }

        public ISyncMessageQueue CreateInboundQueue(string name, MessagePattern pattern)
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

        public ISyncMessageQueue CreateOutboundQueue(string name, MessagePattern pattern)
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

        public IASyncMessageQueue CreateInboundQueueAsync(string name, MessagePattern pattern)
        {
            var key =
                $"{Direction.Inbound}:{name}:{pattern}";
            if (_asyncQueues.ContainsKey(key))
                return _asyncQueues[key];

            var que = CreateMessageQueueAsync();

            que.InitializeInbound(name, pattern);
            _asyncQueues[key] = que;
            return _asyncQueues[key];
        }

        public IASyncMessageQueue CreateOutboundQueueAsync(string name, MessagePattern pattern)
        {
            var key =
                $"{Direction.OutBound}:{name}:{pattern}";
            if (_asyncQueues.ContainsKey(key))
                return _asyncQueues[key];

            var que = CreateMessageQueueAsync();

            que.InitializeInbound(name, pattern);
            _asyncQueues[key] = que;
            return _asyncQueues[key];
        }


        public abstract ISyncMessageQueue CreateMessageQueue();
        public abstract IASyncMessageQueue CreateMessageQueueAsync();
    }
}