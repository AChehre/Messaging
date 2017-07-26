using System;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueue : IDisposable
    {
        string Name { get; }
        string Address { get; }
        IDictionary<string, string> Properties { get; }
        void InitializeOutbound(string name, MessagePattern pattern);
        void InitializeInbound(string name, MessagePattern pattern);
        string GetAddress(string name);
        void Send(Message message);
        void Received(Action<Message> onMessageReceived);
        void ReceivedW(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived, string key);
        IMessageQueue GetResponseQueue();
        IMessageQueue GetReplyQueue(Message message);
    }
}