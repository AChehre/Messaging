using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        public string Name { get; }
        public string Address { get; }
        private IConnection _connection;
        private IModel _channel;
        public IDictionary<string, string> Properties { get; }



        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(RabbitMqMessageQueueConfig config)
        {
            throw new NotImplementedException();
        }

        void IMessageQueue.InitializeInbound(MessageQueueConfig config)
        {
            //
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


        public void Dispose()
        {
            throw new NotImplementedException();
        }


     
    }
}