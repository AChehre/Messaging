using System;
using System.Text;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingServer : IMessagingServer
    {
        private readonly IMessageQueueFactory _factory;
        private readonly IMessageQueue _serverReceive;
        private IMessageQueue _serverSend;
        private string _key;
        public ZeroMqMessagingServer(IMessageQueueFactory factory)
        {
            _factory = factory;

            _serverReceive = factory.CreateInboundQueue(
                new MessageQueueConfig("create-customer-sub", MessagePattern.PublishSubscribe)
                {
                    SubscribeKey = "create"
                });
        }

        public TResult ReceiveRequest<TResult>()
        {
            var result = default(TResult);
            _serverReceive.Received(message =>
            {
                result = message.BodyAs<TResult>();
                _key = Encoding.UTF8.GetString(message.ResponseKey);
            });
            _serverSend = _factory.CreateOutboundQueue("customer-created", MessagePattern.PublishSubscribe);
            return result;
        }

        public void SendResult<TMessage>(TMessage message)
        {
            _serverSend.Send(new Message()
            {
                Body = message
            }, _key);
        }
    }
}