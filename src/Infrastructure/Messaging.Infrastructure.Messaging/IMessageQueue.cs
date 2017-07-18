using System;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueue<TMessageQueueConfig> : IDisposable
        where TMessageQueueConfig : MessageQueueConfig
    {
        string Name { get; }
        string Address { get; }
        IDictionary<string, string> Properties { get; }
        void InitializeOutbound(TMessageQueueConfig messageQueueConfig);
        void InitializeInbound(TMessageQueueConfig messageQueueConfig);

        void Send(Message message);
        void Listen(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived, string key);
        void Received(Action<Message> onMessageReceived);
        string GetAddress(string name);
        IMessageQueue<TMessageQueueConfig> GetResponseQueue();
        IMessageQueue<TMessageQueueConfig> GetReplyQueue(Message message);
    }
}