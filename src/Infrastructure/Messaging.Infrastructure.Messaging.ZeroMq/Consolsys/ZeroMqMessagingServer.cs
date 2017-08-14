using System;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingServer : IMessagingServer
    {
        public void SendResult<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }

        public TResult ReceiveRequest<TResult>()
        {
            throw new NotImplementedException();
        }
    }
}