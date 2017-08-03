using System;
using System.Collections.Generic;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IModel _channel;
        protected MessageQueueConfig _config;

        public RabbitMqMessageQueue(RabbitMqConfig rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public string Address { get; }
        public string GetAddress(string name)
        {
            throw new NotImplementedException();
        }

        public string Name => _config.Name;


        public IDictionary<string, string> Properties { get; }


        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            _config = new MessageQueueConfig(name, pattern);

            _channel = CreateChannel(_config.Name,
                GetExchangeType(_config.MessagePattern));
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            _channel = CreateChannel(Name,
                GetExchangeType(config.MessagePattern, Direction.Inbound, config.Name, config.SubscribeKey));
        }


        public void Send(Message message)
        {
            _channel.BasicPublish(Name,
                "",
                null,
                Encoding.UTF8.GetBytes(message.ToJson()));
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            throw new NotImplementedException();
        }

        public void Listen(Action<Message> onMessageReceived)
        {
            throw new NotImplementedException();
        }

        public void Listen(Action<Message> onMessageReceived, string key)
        {
            throw new NotImplementedException();
        }

        public IMessageQueue GetResponseQueue()
        {
            throw new NotImplementedException();
        }

        public IMessageQueue GetReplyQueue(Message message)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private string GetExchangeType(MessagePattern pattern)
        {
            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    return "fanout";
                case MessagePattern.RequestResponse:
                    return "direct";
                case MessagePattern.PublishSubscribe:
                    return "topic";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }


        private string GetExchangeType(MessagePattern pattern, Direction direction, string name, string key)
        {
            if (pattern == MessagePattern.FireAndForget && key.IsNullOrEmpty())
                return "fanout";

            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    return "fanout";
                case MessagePattern.RequestResponse:
                    return "direct";
                case MessagePattern.PublishSubscribe:
                    return "topic";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }


        private IModel CreateChannel(string exchangeName, string exchangeType)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.UserName,
                Password = _rabbitMqConfig.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

           
            return channel;
        }
    }
}