using System;
using System.Collections.Generic;
using System.Linq;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqMessageQueueFactory : MessageQueueFactory
    {
        public override IMessageQueue CreateMessageQueue()
        {
            var bindings = new RabbitMqBinding()
            {
                new RabbitMqBindingItem("fanout-exchange", "fanout-exchange-queue", ""),

            };

            return new RabbitMqMessageQueue(new RabbitMqConfig("localhost", "guest", "guest")
            {
                CreateExchange = true
            });
        }
    }


   
}