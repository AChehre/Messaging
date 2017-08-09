using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class BaseRabbitMqMessageQueueRpc
    {
        protected readonly RabbitMqConfig RabbitMqConfig;
        protected IModel Channel;
        protected MessageQueueConfig Config;

        public BaseRabbitMqMessageQueueRpc(RabbitMqConfig rabbitMqConfig)
        {
            RabbitMqConfig = rabbitMqConfig;
        }


        public string Address { get; }

        public string Name => Config.Name;


        public IDictionary<string, string> Properties { get; }

        public string GetAddress(string name)
        {
            throw new NotImplementedException();
        }


        protected IModel CreateChannel()
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMqConfig.HostName,
                UserName = RabbitMqConfig.UserName,
                Password = RabbitMqConfig.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();


            return channel;
        }

        protected void CreateAndBindQueue(IModel channel, string exchangeName, string queueName, string routingKey)
        {
            channel.QueueDeclare(queueName, false, false, true, null);
            channel.QueueBind(queueName, exchangeName, routingKey);
        }

        protected string GetExchangeType(MessagePattern pattern)
        {
            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    return "fanout";
                case MessagePattern.RequestResponse:
                    return "direct";
                case MessagePattern.PublishSubscribe:
                    return "fanout";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }


        protected string GetExchangeType(MessagePattern pattern, Direction direction, string name, string key)
        {
            if (pattern == MessagePattern.FireAndForget && key.IsNullOrEmpty())
                return ExchangeType.Fanout;

            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    return ExchangeType.Fanout;
                case MessagePattern.RequestResponse:
                    return ExchangeType.Direct;
                case MessagePattern.PublishSubscribe:
                    return ExchangeType.Fanout;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }

        protected void CreateExchange(IModel channel, string exchangeName, string exchangeType)
        {
            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
        }


        public IMessageQueue GetResponseQueue()
        {
            throw new NotImplementedException();
        }

        public IMessageQueue GetReplyQueue(Message message)
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}