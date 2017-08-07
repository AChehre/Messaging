using System;
using System.Text;
using System.Threading.Tasks;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Microsoft.AspNetCore.Mvc;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerRequest createCustomerRequest)
        {
            CustomerCreatedResponse customerCreatedResponse = null;

            await Task.Run(() =>
            {
                var messageQueueFactory = new ZeroMqMessageQueueFactoryAsync();


                var queue = messageQueueFactory.CreateOutboundQueue("CreateCustomer", MessagePattern.RequestResponse);


                var responseQueue = queue.GetResponseQueue();

                queue.Send(new Message
                {
                    Body = createCustomerRequest,
                    ResponseAddress = responseQueue.Address
                });

                responseQueue.Received(r => customerCreatedResponse = r.BodyAs<CustomerCreatedResponse>());
            });


            if (customerCreatedResponse.Id > 0)
                return Created($"/api/Customers/{customerCreatedResponse.Id}", customerCreatedResponse);


            return BadRequest("Customer does not created.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            CustomerDeletedResponse customerDeletedResponse = null;

            var messageQueueFactory = new ZeroMqMessageQueueFactoryAsync();


            var queue = messageQueueFactory.CreateOutboundQueue("DeleteCustomer", MessagePattern.RequestResponse);


            var responseQueue = queue.GetResponseQueue();


            queue.Send(new Message
            {
                Body = new DeleteCustomerRequest(id),
                ResponseAddress = responseQueue.Address
            });


            responseQueue.Received(r => customerDeletedResponse = r.BodyAs<CustomerDeletedResponse>());

            await Task.Run(() => { });


            return Ok(customerDeletedResponse);
        }


        private CustomerCreatedResponse CustomerCreatedResponse(CreateCustomerRequest createCustomerRequest)
        {
            CustomerCreatedResponse customerCreatedResponse = null;
            var messageQueueFactory = new ZeroMqMessageQueueFactory();


            var queue = messageQueueFactory.CreateOutboundQueue("customer-with-pubsub",
                MessagePattern.PublishSubscribe);
            var key = Guid.NewGuid().ToString();
            var answerqueue =
                messageQueueFactory.CreateInboundQueue(
                    new MessageQueueConfig("customer-with-pubsub-answer", MessagePattern.PublishSubscribe)
                    {
                        SubscribeKey = key
                    });


            queue.Send(new Message
            {
                Body = createCustomerRequest,
                ResponseKey = Encoding.Unicode.GetBytes(key)
            }, "customer-with-pubsub");

            answerqueue.Received(r => customerCreatedResponse = r.BodyAs<CustomerCreatedResponse>());
            return customerCreatedResponse;
        }
    }
}