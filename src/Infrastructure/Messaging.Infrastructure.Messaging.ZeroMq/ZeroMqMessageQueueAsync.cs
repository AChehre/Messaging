using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class BaseZeroMqMessageQueue
    {
        protected ZeroMqMessageQueueConfig _config;

        public string Name => _config.MessageQueueName;

        public string Address => GetAddress(_config.MessageQueueName);

        public IDictionary<string, string> Properties { get; }

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
    }

    public class ZeroMqMessageQueueAsync : BaseZeroMqMessageQueue, IASyncMessageQueue
    {
        private NetMQSocket _socket;


        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            _config = new ZeroMqMessageQueueConfig(name, pattern);
            switch (_config.MessagePattern)
            {
                case MessagePattern.RequestResponse:
                    _socket = new RequestSocket();
                    _socket.Connect(Address);
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
                case MessagePattern.RequestResponse:
                    _socket = new RouterSocket();
                    _socket.Bind(Address);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern),
                        pattern, null);
            }
        }

        public void Send(Message message, string key)
        {
            _socket.SendFrame(message.ToJson());
        }

        public void Received(Action<Message> onMessageReceived)
        {
            var receivedFrame = _socket.ReceiveFrameString();
            var message = receivedFrame.DeserializeFromJson<Message>();
            onMessageReceived(message);
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

        public IASyncMessageQueue GetResponseQueue()
        {
            return this;
        }

        public IASyncMessageQueue GetReplyQueue(Message message)
        {
            return this;
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}