using System;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingClient : IMessagingClient
    {
        public void SendRequest<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }

        public TResult ReceiveResult<TResult>()
        {
            throw new NotImplementedException();
        }
    }
}