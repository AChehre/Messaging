using System;
using System.Collections.Concurrent;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueAsync : BaseZeroMqMessageQueue, IMessageQueue
    {
        private readonly NetMQPoller _poller;
        private readonly NetMQQueue<Action> _queue;
        public ConcurrentQueue<Message> _receiveQueue = new ConcurrentQueue<Message>();
        private NetMQSocket _socket;

        public ZeroMqMessageQueueAsync()
        {
            _queue = new NetMQQueue<Action>();
            _poller = new NetMQPoller {_queue};
            _queue.ReceiveReady += (sender, args) => ProcessCommand(_queue.Dequeue());
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


        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            _config = new ZeroMqMessageQueueConfig(name, pattern);
            switch (_config.MessagePattern)
            {
                case MessagePattern.RequestResponse:
                    _socket = new DealerSocket();
                    _socket.Options.Identity =
                        Encoding.Unicode.GetBytes(Guid.NewGuid().ToString());

                    _socket.Connect(Address);
                    _socket.ReceiveReady += ReceiveReady;
                    _poller.Add(_socket);
                    _poller.RunAsync();

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern),
                        pattern, null);
            }
        }

        public void Send(Message message)
        {
            var multipartMessage = new NetMQMessage();


            if (message.ResponseKey == null || message.ResponseKey.Length == 0)
                _queue.Enqueue(() =>
                {
                    multipartMessage.AppendEmptyFrame();
                    multipartMessage.Append(message.ToJson());
                    _socket.SendMultipartMessage(multipartMessage);
                });
            else
                _queue.Enqueue(() =>
                {
                    multipartMessage.Append(new NetMQFrame(message.ResponseKey));
                    multipartMessage.AppendEmptyFrame();
                    multipartMessage.Append(message.ToJson());
                    _socket.SendMultipartMessage(multipartMessage);
                });
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


        public void ReceivedW(Action<Message> onMessageReceived)
        {
            Message message;
            while (_receiveQueue.TryDequeue(out message))
            {
                onMessageReceived(message);
                return;
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

        private Message Map(NetMQMessage receiveMessage)
        {
            if (receiveMessage.FrameCount > 2)

            {
                var message = receiveMessage[2].ConvertToString().DeserializeFromJson<Message>();
                message.ResponseKey = receiveMessage[0].ToByteArray();
                return message;
            }
            else
            {
                var message = receiveMessage[1].ConvertToString().DeserializeFromJson<Message>();
                return message;
            }
        }

        public void ProcessCommand(Action command)
        {
            command.Invoke();
        }

        private void ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            var clientMessage = new NetMQMessage();

            if (e.Socket.TryReceiveMultipartMessage(ref clientMessage))
                _receiveQueue.Enqueue(Map(clientMessage));
        }
    }
}