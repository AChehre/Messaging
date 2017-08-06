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
        private QueueingBasicConsumer _consumer;
        private string _correlationId;

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
            _config = new MessageQueueConfig(name, pattern);
            _channel = CreateChannel();
            if (_rabbitMqConfig.CreateExchange)
                CreateExchange(_channel, name, GetExchangeType(pattern));

            var _consumer = new QueueingBasicConsumer(_channel);

            _channel.BasicConsume(_channel.QueueDeclare().QueueName,
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
            _config = config;
            _channel = CreateChannel();

            if (!_rabbitMqConfig.CreateQueue) return;

            var bindings = _rabbitMqConfig.Bindings.Get(config.Name);
            foreach (var bindingItem in bindings)
                CreateAndBindQueue(_channel,
                    bindingItem.ExchangeName,
                    bindingItem.QueueName,
                    bindingItem.RoutingKey);
        }

        public void Send(Message message)
        {
            if (_config.MessagePattern == MessagePattern.FireAndForget)
            {
                _channel.BasicPublish(Name,
                    "",
                    null,
                    Encoding.UTF8.GetBytes(message.ToJson()));
            }
            else
            {
                _correlationId = Guid.NewGuid().ToString();
                var props = _channel.CreateBasicProperties();
                props.ReplyTo = _channel.QueueDeclare().QueueName;
                props.CorrelationId = _correlationId;

                var messageBytes = Encoding.UTF8.GetBytes(message.ToJson());
                _channel.BasicPublish("",
                    _config.Name,
                    props,
                    messageBytes);
            }
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            while (true)
            {
                var ea = _consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == _correlationId)
                    onMessageReceived.Invoke(Encoding.UTF8.GetString(ea.Body).DeserializeFromJson<Message>());
            }
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
            //ea.BasicProperties.CorrelationId

            //_messageQueue.TryAdd()

            //if (_onMessageReceived == null) return;
            //var body = ea.Body;
            //var jsonObject = Encoding.UTF8.GetString(body);
            //var message = jsonObject.DeserializeFromJson<Message>();

            //_onMessageReceived.Invoke(message);
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
