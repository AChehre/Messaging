using System;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueAsync : BaseZeroMqMessageQueue, IMessageQueue
    {
        private NetMQSocket _socket;


        public void InitializeInbound(string name, MessagePattern pattern)
        {
            var config = new MessageQueueConfig(name, pattern);
            InitializeInbound(config);
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            _config = config;

            switch (_config.MessagePattern)
            {
                case MessagePattern.RequestResponse:
                    _socket = new RouterSocket();
                    _socket.Bind(Address);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(config.MessageQueueName),
                        config.MessagePattern, null);
            }
        }


        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            _config = new MessageQueueConfig(name, pattern);
            switch (_config.MessagePattern)
            {
                case MessagePattern.RequestResponse:
                    _socket = new RequestSocket();
                    _socket.Connect(Address);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(name),
                        pattern, null);
            }
        }

        public void Send(Message message)
        {
            var multipartMessage = new NetMQMessage();


            if (message.ResponseKey == null || message.ResponseKey.Length == 0)
            {
                multipartMessage.Append(message.ToJson());
                _socket.SendMultipartMessage(multipartMessage);
            }
            else
            {
                multipartMessage.Append(new NetMQFrame(message.ResponseKey));
                multipartMessage.AppendEmptyFrame();
                multipartMessage.Append(message.ToJson());
                _socket.SendMultipartMessage(multipartMessage);
            }
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
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
            var receiveMessage = _socket.ReceiveMultipartMessage();
            onMessageReceived(Map(receiveMessage));
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

        private Message Map(NetMQMessage receiveMessage)
        {
            if (receiveMessage.FrameCount > 1)
            {
                var message = receiveMessage[2].ConvertToString().DeserializeFromJson<Message>();
                message.ResponseKey = receiveMessage[0].ToByteArray();
                return message;
            }

            else
            {
                var message = receiveMessage[0].ConvertToString().DeserializeFromJson<Message>();
                return message;
            }
        }
    }
}