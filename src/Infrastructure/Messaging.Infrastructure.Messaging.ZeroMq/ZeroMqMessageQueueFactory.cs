using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueFactory : MessageQueueFactory
    {
        public override IMessageQueue CreateMessageQueue()
        {
            return new ZeroMqMessageQueue();
        }
    }
}
