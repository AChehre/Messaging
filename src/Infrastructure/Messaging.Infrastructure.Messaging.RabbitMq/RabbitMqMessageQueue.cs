using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        private IModel _channel;

        //private IConnection _connection;
        public string Name { get; }

        public string Address { get; }

        public IDictionary<string, string> Properties { get; }


        public void InitializeOutbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            throw new NotImplementedException();
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            _channel = CreateChannel(config.RabbitMq.HostName, config.RabbitMq.ExchangeName,
                GetExchangeType(config.MessagePattern));
        }


        public string GetAddress(string name)
        {
            throw new NotImplementedException();
        }

        public void Send(Message message)
        {
            throw new NotImplementedException();
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
                    break;
                case MessagePattern.RequestResponse:
                    return "direct";
                    break;
                case MessagePattern.PublishSubscribe:
                    return "topic";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }

        public void InitializeInbound(RabbitMqMessageQueueConfig config)
        {
            throw new NotImplementedException();
        }


        private IModel CreateChannel(string hostName, string exchangeName, string exchangeType)
        {
            //var hostName = hostName ?? GetProperty(HostNamePropertyName, "localhost");
            var factory = new ConnectionFactory {HostName = hostName};

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchangeName, exchangeType);
            return channel;
        }
    }
}