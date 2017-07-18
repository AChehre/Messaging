using System;

namespace Messaging.Infrastructure.Messaging
{
    //TODO: Chehre >> Change type to Value Object 
    public class Message
    {

        //public Type BodyType => Body.GetType();

        public object Body { get; set; }

        public string ResponseAddress { get; set; }
        //public string MessageType { get; set; }

        public TBody BodyAs<TBody>()
        {
            return (TBody) Body;
        }
    }
}