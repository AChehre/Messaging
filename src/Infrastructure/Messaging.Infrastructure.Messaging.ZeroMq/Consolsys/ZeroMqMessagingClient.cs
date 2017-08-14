using System;
using System.Text;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingClient : IMessagingClient
    {
        private readonly IMessageQueue _clientSend;
        private readonly IMessageQueueFactory _factory;
        private IMessageQueue _clientReceive;
        private string _key;


        public ZeroMqMessagingClient(IMessageQueueFactory factory)
        {
            _factory = factory;
            _clientSend = factory.CreateOutboundQueue("create-customer-pub", MessagePattern.PublishSubscribe);
        }

        public void SendRequest<TMessage>(TMessage message)
        {
            _key = Guid.NewGuid().ToString();
            _clientReceive = _factory.CreateInboundQueue(
                new MessageQueueConfig("customer-created", MessagePattern.PublishSubscribe)
                {
                    SubscribeKey = _key
                });

            var sendMessage = new Message
            {
                Body = message,
                ResponseKey = Encoding.UTF8.GetBytes(_key)
            };

            _clientSend.Send(sendMessage, "create");
        }

        public TResult ReceiveResult<TResult>()
        {
            var result = default(TResult);
            _clientReceive.Received(message => { result = message.BodyAs<TResult>(); });
            return result;
        }
    }
}