using System;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging
{
    public interface IMessageQueue : IDisposable
    {
        string Name { get; }
        string Address { get; }
        IDictionary<string, string> Properties { get; }
        string GetAddress(string name);
    }

    public interface ISyncMessageQueue : IMessageQueue
    {
        void InitializeOutbound(string name, MessagePattern pattern);
        void InitializeInbound(string name, MessagePattern pattern);
        void Send(Message message);
        void Received(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived, string key);
        ISyncMessageQueue GetResponseQueue();
        ISyncMessageQueue GetReplyQueue(Message message);
    }

    public interface IASyncMessageQueue : IMessageQueue
    {
        void InitializeOutbound(string name, MessagePattern pattern);
        void InitializeInbound(string name, MessagePattern pattern);
        void Send(Message message, string key);
        void Received(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived);
        void Listen(Action<Message> onMessageReceived, string key);
        IASyncMessageQueue GetResponseQueue();
        IASyncMessageQueue GetReplyQueue(Message message);
    }
}