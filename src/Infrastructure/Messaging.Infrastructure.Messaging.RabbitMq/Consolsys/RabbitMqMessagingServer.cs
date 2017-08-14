using System;
using System.Collections.Generic;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.RabbitMq.Consolsys
{
    public class RabbitMqMessagingServer : IMessagingServer
    {
        private readonly IMessageQueue _server;
        private string _responseAddress;
        private  IDictionary<string, object> _properties;
        private byte[] _responseKey;

        public RabbitMqMessagingServer(IMessageQueueFactory factory)
        {
            _server = factory.CreateInboundQueue("rpc_queue", MessagePattern.FireAndForget);
        }


        public void ReceiveRequest<TRequest>(Action<TRequest> onRequestReceived)
        {
         
            _server.Received(message => { OnReceive(onRequestReceived, message); });
         
        }

        private void OnReceive<TRequest>(Action<TRequest> onRequestReceived, Message message)
        {
            var result = message.BodyAs<TRequest>();
            _responseAddress = message.ResponseAddress;
            _properties = message.Properties;
            _responseKey = message.ResponseKey;
            onRequestReceived.Invoke(result);
        }

        public void SendResult<TMessage>(TMessage message)
        {

            _server.Send(new Message
            {
                Body = message,
                ResponseAddress = _responseAddress,
                Properties = _properties,
                ResponseKey = _responseKey
            });
        }
    }
}