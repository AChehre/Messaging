﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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


    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>

    {
        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;
            var t = GetType();
            var otherType = other.GetType();

            if (t != otherType)
                return false;

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (!value1.Equals(value2))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as T;
            return Equals(other);
        }


        public override int GetHashCode()
        {
            var fields = GetFields();

            const int startValue = 17;
            const int multiplier = 59;

            return fields.Select(field => field.GetValue(this)).Where(value => value != null).Aggregate(startValue,
                (current, value) => current * multiplier + value.GetHashCode());
        }


        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();
            var fields = new List<FieldInfo>();
            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                t = t.GetTypeInfo().BaseType;
            }

            return fields;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            return x.Equals(y);
        }


        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}