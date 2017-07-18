using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueue : IMessageQueue
    {
        private NetMQSocket _socket;

        public string Name { get; }
        public string Address { get; }
        public IDictionary<string, string> Properties { get; }

        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    _socket = new PushSocket();
                    _socket.Connect(Address);
                    break;

                case MessagePattern.RequestResponse:
                    _socket = new RequestSocket();
                    _socket.Connect(Address);
                    break;

                case MessagePattern.PublishSubscribe:
                    _socket = new PublisherSocket();
                    _socket.Bind(Address);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern),
                        pattern, null);
            }
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    _socket = new PullSocket();
                    _socket.Bind(Address);
                    break;

                case MessagePattern.RequestResponse:
                    _socket = new ResponseSocket();
                    _socket.Bind(Address);
                    break;

                case MessagePattern.PublishSubscribe:
                    var socket = new SubscriberSocket();
                    socket.Connect(Address);
                    socket.Subscribe("");
                    _socket = socket;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern),
                        pattern, null);
            }
        }

        public void Send(Message message)
        {
            _socket.SendFrame(message.ToJson());
        }

        public void Listen(Action<Message> onMessageReceived)
        {
            while (true)
                Received(onMessageReceived);
        }

        public void Listen(Action<Message> onMessageReceived, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            var inbound = _socket.ReceiveFrameString();
            var message = inbound.DeserializeFromJson<Message>();
            onMessageReceived(message);
        }

        public string GetAddress(string name)
        {
            throw new NotImplementedException();
        }

        public IMessageQueue GetResponseQueue()
        {
            return this;
        }

        public IMessageQueue GetReplyQueue(Message message)
        {
            return this;
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}