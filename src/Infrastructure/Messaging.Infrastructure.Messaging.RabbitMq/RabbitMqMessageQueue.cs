using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
   public class RabbitMqMessageQueue: IMessageQueue
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public string Address { get; }
        public IDictionary<string, string> Properties { get; }
        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            throw new NotImplementedException();
        }

        public string GetAddress(string name)
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
            throw new NotImplementedException();
        }

        public void Listen(Action<Message> onMessageReceived)
        {
            throw new NotImplementedException();
        }

        public void Listen(Action<Message> onMessageReceived, string key)
        {
            throw new NotImplementedException();
        }

        public IMessageQueue GetResponseQueue()
        {
            throw new NotImplementedException();
        }

        public IMessageQueue GetReplyQueue(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
