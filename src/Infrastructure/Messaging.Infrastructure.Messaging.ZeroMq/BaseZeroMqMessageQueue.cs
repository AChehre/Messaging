﻿using System;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class BaseZeroMqMessageQueue
    {
        protected MessageQueueConfig _config;

        public string Name => _config.MessageQueueName;

        public string Address => GetAddress(_config.MessageQueueName);

        public IDictionary<string, string> Properties { get; }

        public string GetAddress(string name)
        {
            switch (name.ToLower())
            {
                case "createcustomer":
                    return "tcp://localhost:5555";
                case "deletecustomer":
                    return "tcp://localhost:5556";
                case "customer":
                    return "tcp://localhost:5557";
                case "loadtestrepreq":
                    return "tcp://localhost:5558";
                case "customer-with-pubsub":
                    return "tcp://localhost:5559";
                case "customer-with-pubsub-answer":
                    return "tcp://localhost:5560";
                default:
                    throw new ArgumentException($"Unknown queue name {name}");
            }
        }
    }
}