using System.Threading;
using Messaging.Infrastructure.Messaging;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.ConsoleApp
{
    public class CustomerService
    {
        public void CreateCustomer(IMessageQueue messageQueue, Message message)
        {
            var customerName = message.BodyAs<CreateCustomerRequest>().Name;


            // The Process ...
            Thread.Sleep(5000);


            // Create Customer 
            var id = 1000;

            var customerCreatedResponse = new CustomerCreatedResponse
            {
                Id = id //Created customer Id
            };


            var replyQueue = messageQueue.GetReplyQueue(message);

            replyQueue.Send(new Message
            {
                Body = customerCreatedResponse,
                ResponseAddress = message.ResponseAddress,
                ResponseKey = message.ResponseKey
            });
        }

        public void DeleteCustomer(IMessageQueue messageQueue, Message message)
        {
            var customerId = message.BodyAs<DeleteCustomerRequest>().Id;


            // The Process ...
            Thread.Sleep(1000);


            // Delete Customer 
            var deleted = true;

            var customerdeletedResponse = new CustomerDeletedResponse
            {
                Deleted = deleted
            };


            var replyQueue = messageQueue.GetReplyQueue(message);

            replyQueue.Send(new Message
            {
                Body = customerdeletedResponse,
                ResponseAddress = message.ResponseAddress,
                ResponseKey = message.ResponseKey
                
            });
        }
    }
}