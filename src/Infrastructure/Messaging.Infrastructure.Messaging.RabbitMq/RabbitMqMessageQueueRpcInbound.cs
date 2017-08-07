using System;
using System.Text;
using Messaging.Infrastructure.Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueRpcInbound : BaseRabbitMqMessageQueueRpc, IMessageQueue
    {
        private Action<Message> _onMessageReceived;

        public RabbitMqMessageQueueRpcInbound(RabbitMqConfig rabbitMqConfig) : base(rabbitMqConfig)
        {
        }


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
            Config = config;
            Channel = CreateChannel();


            Channel.QueueDeclare(Config.Name, false,
                false, false, null);
            Channel.BasicQos(0, 1, false);
            var _consumer = new EventingBasicConsumer(Channel);
            Channel.BasicConsume(Config.Name,
                false, _consumer);

            _consumer.Received += OnRabbitMqReceived;
        }

        public void Send(Message message)
        {
            var replyProps = Channel.CreateBasicProperties();
            replyProps.CorrelationId = Encoding.UTF8.GetString(message.ResponseKey);


            var responseBytes = Encoding.UTF8.GetBytes(message.ToJson());
            Channel.BasicPublish("", message.ResponseAddress,
                replyProps, responseBytes);

            var deliveryTag = Convert.ToUInt64(message.Properties["DeliveryTag"]);

            Channel.BasicAck(deliveryTag, false);
        }

        public void Send(Message message, string key)
        {
            throw new NotImplementedException();
        }

        public void Received(Action<Message> onMessageReceived)
        {
            _onMessageReceived = onMessageReceived;
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
    }
}