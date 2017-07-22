using System;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueAsync : BaseZeroMqMessageQueue, IMessageQueue
    {
        private NetMQMessage _lastNetMQMessage;
        private NetMQSocket _socket;

        public void Send(Message message)
        {
            var multipartMessage = new NetMQMessage();
            if (_lastNetMQMessage != null)
            {
                multipartMessage.Append(_lastNetMQMessage[0]);
                multipartMessage.AppendEmptyFrame();
                multipartMessage.Append(message.ToJson());
                _socket.SendMultipartMessage(multipartMessage);
                _lastNetMQMessage = null;
            }

            multipartMessage.Append(message.ToJson());
            _socket.SendMultipartMessage(multipartMessage);
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
            _lastNetMQMessage = _socket.ReceiveMultipartMessage();
            var message = _lastNetMQMessage[2].ConvertToString().DeserializeFromJson<Message>();
            onMessageReceived(message);
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