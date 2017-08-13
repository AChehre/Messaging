using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueue : BaseZeroMqMessageQueue, IMessageQueue
    {
        private NetMQSocket _socket;

        public ZeroMqMessageQueue()
        {
            
        }
        public ZeroMqMessageQueue(Dictionary<string, string> addressMapping) : base(addressMapping)
        {
        }

        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            Config = new MessageQueueConfig(name, pattern);
            switch (Config.MessagePattern)
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
                    _socket = new PublisherSocket(Address);
                    //_socket.Bind(Address);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern),
                        pattern, null);
            }
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            var config = new MessageQueueConfig(name, pattern);
            InitializeInbound(config);
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            Config = config;
            switch (Config.MessagePattern)
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
                    var socket = new SubscriberSocket(Address);
                    //socket.Connect(Address);
                    //socket.Options.SendHighWatermark = 1000;
                    socket.Subscribe(Config.SubscribeKey);
                    _socket = socket;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(config.Name),
                        config.MessagePattern, null);
            }
        }

        public void Send(Message message)
        {
            var multipartMessage = new NetMQMessage();
            multipartMessage.Append(message.ToJson());

            if (Config.MessagePattern == MessagePattern.PublishSubscribe)
                _socket.SendMoreFrame("").SendMultipartMessage(multipartMessage);
            else
                _socket.SendMultipartMessage(multipartMessage);
        }

        public void Send(Message message, string key)
        {
            var multipartMessage = new NetMQMessage();
            multipartMessage.Append(message.ToJson());

            if (Config.MessagePattern == MessagePattern.PublishSubscribe)
                _socket.SendMoreFrame(key).SendMultipartMessage(multipartMessage);
            else
                _socket.SendMultipartMessage(multipartMessage);
        }

        public void Received(Action<Message> onMessageReceived)
        {
            NetMQMessage receiveMessage;

            if (Config.MessagePattern == MessagePattern.PublishSubscribe)
            {
                var messageTopicReceived = _socket.ReceiveFrameString();
                receiveMessage = _socket.ReceiveMultipartMessage();
                var message = receiveMessage[0].ConvertToString().DeserializeFromJson<Message>();
                onMessageReceived(message);
            }
            else
            {
                receiveMessage = _socket.ReceiveMultipartMessage();
                var message = receiveMessage[0].ConvertToString().DeserializeFromJson<Message>();
                onMessageReceived(message);
            }
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