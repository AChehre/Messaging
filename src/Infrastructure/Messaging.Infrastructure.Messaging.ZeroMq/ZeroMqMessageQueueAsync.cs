using System;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueAsync : BaseZeroMqMessageQueue, IMessageQueue
    {
        private NetMQSocket _socket;


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


            if (receiveMessage.FrameCount > 1)

            {
                var message = receiveMessage[2].ConvertToString().DeserializeFromJson<Message>();
                message.ResponseKey = receiveMessage[0].ToByteArray();
                onMessageReceived(message);
            }
            else
            {
                var message = receiveMessage[0].ConvertToString().DeserializeFromJson<Message>();
                onMessageReceived(message);
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
    }
}