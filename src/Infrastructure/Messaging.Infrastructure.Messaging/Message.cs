using System;
using Newtonsoft.Json.Linq;

namespace Messaging.Infrastructure.Messaging
{
    //TODO: Chehre >> Change type to Value Object 
    public class Message
    {
        public Type BodyType => Body.GetType();

        public object Body { get; set; }

        public string ResponseAddress { get; set; }

        public string MessageType { get; set; }

        public TBody BodyAs<TBody>()
        {
            if (Body is JObject)
            {
                var jBody = (JObject) Body;
                return jBody.ToObject<TBody>();
            }
            return (TBody) Body;
        }
    }
}