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


        protected void CreateExchange(string exchangeName)
        {
            CreateRabbitExchange(exchangeName, GetExchangeType(Config.MessagePattern));
        }

        protected void CreateQueue(string queueName)
        {
            var bindingItems = RabbitMqConfig.Bindings.Get(queueName);
            foreach (var binding in bindingItems)
            {
                CreateRabbitExchange(binding.ExchangeName, binding.ExchangeType);
                CreateAndBindRabbitMqQueue(binding.ExchangeName, binding.QueueName, binding.RoutingKey);
            }
        }


        protected void CreateAndBindRabbitMqQueue(string exchangeName, string queueName, string routingKey)
        {
            CreateRabbitQueue(queueName);
            BindRabbitQueue(exchangeName, queueName, routingKey);
        }


        protected void CreateRabbitQueue(string queueName)
        {
            Channel.QueueDeclare(queueName, false, false, true, null);
        }

        protected void BindRabbitQueue(string exchangeName, string queueName, string routingKey)
        {
            Channel.QueueBind(queueName, exchangeName, routingKey);
        }


        protected string GetExchangeType(MessagePattern pattern)
        {
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

        protected void CreateRabbitExchange(string exchangeName, string exchangeType)
        {
            Channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
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