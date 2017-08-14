using System;

namespace Messaging.Infrastructure.Messaging.Consolsys
{
    public interface IMessagingServer
    {
        void ReceiveRequest<TRequest>(Action<TRequest> onRequestReceived);
        void SendResult<TMessage>(TMessage message);
      
    }
}