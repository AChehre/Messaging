using System;
using Newtonsoft.Json.Linq;

namespace Messaging.Infrastructure.Messaging
{
    //TODO: Chehre >> Change type to Value Object 
    public class Message
    {
        public Type BodyType => _body.GetType();

        private object _body;

        public object Body
        {
            get { return _body; }
            set
            {
                _body = value;
                MessageType = BodyType.Name;
            }
        }

        public string ResponseAddress { get; set; }
        public byte[] ResponseKey { get; set; }

        public string MessageType { get; set; }

        public TBody BodyAs<TBody>()
        {
            if (Body is JObject)
            {
                var jBody = (JObject)Body;
                return jBody.ToObject<TBody>();
            }
            return (TBody)Body;
        }
    }
}