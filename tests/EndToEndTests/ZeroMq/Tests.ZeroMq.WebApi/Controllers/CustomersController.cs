using System;
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
       

            var messageQueueFactory = new ZeroMqMessageQueueFactory();


            var queue = messageQueueFactory.CreateOutboundQueue("CreateCustomer", MessagePattern.RequestResponse);


            var responseQueue = queue.GetResponseQueue();

            queue.Send(new Message
            {
                Body = createCustomerRequest,
                ResponseAddress = responseQueue.Address
            });

            var createdCustomerId = 0;


            responseQueue.Received(r => createdCustomerId = r.BodyAs<CustomerCreatedResponse>().Id);

            if (createdCustomerId > 0)
                return Created($"/api/Customers/{createdCustomerId}", createdCustomerId);


            return BadRequest("Customer does not created.");
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}