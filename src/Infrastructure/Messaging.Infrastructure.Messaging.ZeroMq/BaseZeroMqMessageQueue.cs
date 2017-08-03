using System;
using System.Collections.Generic;

namespace Messaging.Infrastructure.Messaging.ZeroMq
{
    public class BaseZeroMqMessageQueue
    {
        public BaseZeroMqMessageQueue()
        {
            
        }
        public BaseZeroMqMessageQueue(Dictionary<string, string> addressMapping)
        {
            _addressMapping = addressMapping;
        }
        private readonly Dictionary<string, string> _addressMapping;

        protected MessageQueueConfig _config;

        public string Name => _config.Name;

        public string Address => GetAddress(_config.Name);

        public IDictionary<string, string> Properties { get; }

        public string GetAddress(string name)
        {
            
            //TODO: throw exception if name or address is null or not found
            return _addressMapping[name];
        }
    }
}