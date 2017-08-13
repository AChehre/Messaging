using System;
using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingClient : IMessagingClient
    {
        public void Send<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }

        public TResult Receive<TResult>()
        {
            throw new NotImplementedException();
        }
    }
}