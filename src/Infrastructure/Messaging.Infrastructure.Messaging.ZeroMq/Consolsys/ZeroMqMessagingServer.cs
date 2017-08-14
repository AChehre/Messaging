using System;
using System.Text;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingServer : IMessagingServer
    {
        private readonly IMessageQueueFactory _factory;
        private readonly IMessageQueue _serverReceive;
        private string _key;
        private IMessageQueue _serverSend;

        public ZeroMqMessagingServer(IMessageQueueFactory factory)
        {
            _factory = factory;

            _serverReceive = factory.CreateInboundQueue(
                new MessageQueueConfig("create-customer-sub", MessagePattern.PublishSubscribe)
                {
                    SubscribeKey = "create"
                });
        }

        public void ReceiveRequest<TRequest>(Action<TRequest> onRequestReceived)
        {
            var result = default(TRequest);
            _serverReceive.Received(message =>
            {
                result = message.BodyAs<TRequest>();
                _key = Encoding.UTF8.GetString(message.ResponseKey);
            });
            _serverSend = _factory.CreateOutboundQueue("customer-created", MessagePattern.PublishSubscribe);
            onRequestReceived.Invoke(result);
        }

        public void SendResult<TMessage>(TMessage message)
        {
            _serverSend.Send(new Message
            {
                Body = message
            }, _key);
        }
    }
}