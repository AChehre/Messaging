using System;
using System.Collections.Generic;
using System.Linq;
using Messaging.Infrastructure.Common;

namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqBindingItem : ValueObject<RabbitMqBindingItem>
    {
        public RabbitMqBindingItem(string exchangeName, string queueName, string routingKey)
        {
            if (string.IsNullOrWhiteSpace(exchangeName))
                throw new ArgumentNullException(nameof(exchangeName));

            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentNullException(nameof(queueName));

            QueueName = queueName;
            RoutingKey = routingKey;
            ExchangeName = exchangeName;
        }

        public string ExchangeName { get; }
        public string QueueName { get; }
        public string RoutingKey { get; }
    }


    public class RabbitMqBinding : HashSet<RabbitMqBindingItem>
    {
        //private readonly HashSet<RabbitMqBindingItem> _bindings;

        //public RabbitMqBinding()
        //{
        //    _bindings = new HashSet<RabbitMqBindingItem>();
        //}


        //public void Add(RabbitMqBindingItem binding)
        //{
        //    if (!_bindings.Add(binding))
        //        throw new ArgumentException(nameof(binding));
        //}

        public bool Has(RabbitMqBindingItem binding)
        {
            return Contains(binding);
        }


        public IEnumerable<RabbitMqBindingItem> Get()
        {
            return this.ToList();
        }

        public IEnumerable<RabbitMqBindingItem> Get(string queueName)
        {
            return this.ToList().Where(b => b.QueueName == queueName);
        }
    }
}