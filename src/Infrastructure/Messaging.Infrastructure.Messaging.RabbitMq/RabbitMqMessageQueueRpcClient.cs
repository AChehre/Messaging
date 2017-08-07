using System;
using System.Collections.Generic;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueRpcClient : IMessageQueue
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IModel _channel;
        private MessageQueueConfig _config;
        private QueueingBasicConsumer _consumer;
        private string _correlationId;
        private string _queueName;
        private string _replyQueueName;

        //private Action<Message> _onMessageReceived;

        public RabbitMqMessageQueueRpcClient(RabbitMqConfig rabbitMqConfig)
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
            _channel = CreateChannel();

            _queueName = "rpc_queue";

            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume(_replyQueueName,
                true,
                _consumer);
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            var config = new MessageQueueConfig(name, pattern);
            InitializeInbound(config);
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
        }

        public void Send(Message message)
        {
            _correlationId = Guid.NewGuid().ToString();
            var props = _channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = _correlationId;

            var messageBytes = Encoding.UTF8.GetBytes(message.ToJson());
            _channel.BasicPublish("",
                _queueName,
                props,
                messageBytes);
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            //while (true)
            //{
                var ea = _consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId != _correlationId)
                    return;
                var result = Encoding.UTF8.GetString(ea.Body);
                onMessageReceived.Invoke(result.DeserializeFromJson<Message>());
            //}
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

        private void OnRabbitMqReceived(object model, BasicDeliverEventArgs ea)
        {
        }

        private void CreateAndBindQueue(IModel channel, string exchangeName, string queueName, string routingKey)
        {
            channel.QueueDeclare(queueName, false, false, true, null);
            channel.QueueBind(queueName, exchangeName, routingKey);
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
                return ExchangeType.Fanout;

            switch (pattern)
            {
                case MessagePattern.FireAndForget:
                    return ExchangeType.Fanout;
                case MessagePattern.RequestResponse:
                    return ExchangeType.Direct;
                case MessagePattern.PublishSubscribe:
                    return ExchangeType.Topic;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }

        private void CreateExchange(IModel channel, string exchangeName, string exchangeType)
        {
            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
        }


        private IModel CreateChannel()
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