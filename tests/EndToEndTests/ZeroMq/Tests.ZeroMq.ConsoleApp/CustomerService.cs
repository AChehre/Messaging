using System.Threading;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.ConsoleApp
{
    public class CustomerService
    {
        public void CreateCustomer(IMessageQueue messageQueue, Message message)
        {
            var customerName = message.BodyAs<CreateCustomerRequest>().Name;

            Common.Show($"Message received {customerName}");


            var sleepTime = 5000;

            Common.Show($"Start process {customerName} for {sleepTime} min");

            // The Process ...
            Thread.Sleep(sleepTime);


            
            // Create Customer 
            var id = 1000;

            var customerCreatedResponse = new CustomerCreatedResponse
            {
                Id = id //Created customer Id
            };


            var replyQueue = messageQueue.GetReplyQueue(message);


            Common.Show($"Send message {customerName}");
            replyQueue.Send(new Message
            {
                Body = customerCreatedResponse,
                ResponseAddress = message.ResponseAddress,
                ResponseKey = message.ResponseKey
            });

            Common.Separator();
        }

        public void DeleteCustomer(IMessageQueue messageQueue, Message message)
        {
            var customerId = message.BodyAs<DeleteCustomerRequest>().Id;


            Common.Show($"Message received {customerId}");


            var sleepTime = 1000;

            Common.Show($"Start process {customerId} for {sleepTime} min");

            // The Process ...
            Thread.Sleep(sleepTime);


            // Delete Customer 
            var deleted = true;

            var customerdeletedResponse = new CustomerDeletedResponse
            {
                Deleted = deleted
            };


            var replyQueue = messageQueue.GetReplyQueue(message);

            Common.Show($"Send message {customerId}");
            replyQueue.Send(new Message
            {
                Body = customerdeletedResponse,
                ResponseAddress = message.ResponseAddress,
                ResponseKey = message.ResponseKey
                
            });

            Common.Separator();
        }
    }
}