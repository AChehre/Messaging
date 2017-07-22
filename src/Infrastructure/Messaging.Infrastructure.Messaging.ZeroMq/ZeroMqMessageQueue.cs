using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueue : IMessageQueue
    {
        private ZeroMqMessageQueueConfig _config;
        private NetMQSocket _socket;

        public string Name => _config.MessageQueueName;

        public string Address => GetAddress(_config.MessageQueueName);

        public IDictionary<string, string> Properties { get; }

        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            _config = new ZeroMqMessageQueueConfig(name, pattern);
            switch (_config.MessagePattern)
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
            _config = new ZeroMqMessageQueueConfig(name, pattern);
            switch (_config.MessagePattern)
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
            var receivedFrame = _socket.ReceiveFrameString();
            var message = receivedFrame.DeserializeFromJson<Message>();
            onMessageReceived(message);
        }

        public string GetAddress(string name)
        {
            switch (name.ToLower())
            {
                case "createcustomer":
                    return "tcp://localhost:5555";
                case "deletecustomer":
                    return "tcp://localhost:5556";
                case "customer":
                    return "tcp://localhost:5557";
                default:
                    throw new ArgumentException($"Unknown queue name {name}");
            }
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