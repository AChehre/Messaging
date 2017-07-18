using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class ZeroMqMessageQueueConfig : MessageQueueConfig
    {
        public ZeroMqMessageQueueConfig(string name, MessagePattern pattern)
        {
            MessageQueueName = name;
            MessagePattern = pattern;
        }
    }
}
