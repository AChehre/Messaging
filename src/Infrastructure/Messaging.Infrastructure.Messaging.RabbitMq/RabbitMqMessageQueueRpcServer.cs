using System;
using System.Collections.Generic;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueRpcServer : IMessageQueue
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IModel _channel;
        private MessageQueueConfig _config;
        private Action<Message> _onMessageReceived;


        //private Action<Message> _onMessageReceived;

        public RabbitMqMessageQueueRpcServer(RabbitMqConfig rabbitMqConfig)
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
        }

        public void InitializeInbound(string name, MessagePattern pattern)
        {
            var config = new MessageQueueConfig(name, pattern);
            InitializeInbound(config);
        }

        public void InitializeInbound(MessageQueueConfig config)
        {
            _config = config;
            _channel = CreateChannel();


            _channel.QueueDeclare(_config.Name, false,
                false, false, null);
            _channel.BasicQos(0, 1, false);
            var _consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(_config.Name,
                false, _consumer);

            _consumer.Received += OnRabbitMqReceived;
        }

        public void Send(Message message)
        {
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = Encoding.UTF8.GetString(message.ResponseKey);


            var responseBytes = Encoding.UTF8.GetBytes(message.ToJson());
            _channel.BasicPublish("", message.ResponseAddress,
                replyProps, responseBytes);

            var deliveryTag = Convert.ToUInt64(message.Properties["DeliveryTag"]);

            _channel.BasicAck(deliveryTag, false);
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            _onMessageReceived = onMessageReceived;
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
            var body = ea.Body;
            var props = ea.BasicProperties;

            var stringBody = Encoding.UTF8.GetString(body);

            var message = stringBody.DeserializeFromJson<Message>();
            message.ResponseAddress = props.ReplyTo;
            message.ResponseKey = Encoding.UTF8.GetBytes(props.CorrelationId);

            message.Properties.Add("DeliveryTag", ea.DeliveryTag.ToString());

            _onMessageReceived?.Invoke(message);
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