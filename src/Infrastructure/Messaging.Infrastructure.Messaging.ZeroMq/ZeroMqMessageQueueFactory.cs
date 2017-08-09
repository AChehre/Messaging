using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueFactory : MessageQueueFactory
    {
        public override IMessageQueue CreateMessageQueue(Direction direction)
        {
            return new ZeroMqMessageQueue(new Dictionary<string, string>
            {
                {"createcustomer", "tcp://localhost:5555"},
                {"deletecustomer", "tcp://localhost:5556"},
                {"customer", "tcp://localhost:5557"},
                {"loadtestrepreq", "tcp://localhost:5558"},
                {"customer-with-pubsub", "tcp://localhost:5559"},
                {"customer-with-pubsub-answer", "tcp://localhost:5560"},
                {"mix-customer", "tcp://localhost:5561"},
                {"mix-publish", "tcp://localhost:5562"},
                {"create-customer","epgm://localhost:5563" }
            });
        }
    }
}