using System;
using System.Collections.Generic;
using System.Text;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.ConsoleApp
{
    public class CustomerService
    {
        public void CreateCustomer(IMessageQueue messageQueue, Message message)
        {

            var customerName = message.BodyAs<CreateCustomerRequest>().Name;
            // Create Customer 
            var customerCreatedResponse = new CustomerCreatedResponse()
            {
                Id = 10  //Created customer Id
            };


            var replyQueue = messageQueue.GetReplyQueue(message);

            replyQueue.Send(new Message()
            {
                Body = customerCreatedResponse,
                ResponseAddress = message.ResponseAddress

            });

        }

    }
}
