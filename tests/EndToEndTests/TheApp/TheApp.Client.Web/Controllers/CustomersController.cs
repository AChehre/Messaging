using Messaging.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace TheApp.Client.Web.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly IMessageQueueFactory _factory;

        public CustomersController(MessageQueueFactory factory)
        {
            _factory = factory;
        }


        [HttpPost]
        public IActionResult Post(string customerName)
        {
            var client = _factory.CreateOutboundQueue("customer", MessagePattern.PublishSubscribe);

            var message = new Message
            {
                Body = customerName
            };

            client.Send(message, "create");

            //var result = "";

            //client.Received(messsage => { result = messsage.BodyAs<string>(); });


            //if (string.Equals(result, "OK", StringComparison.CurrentCultureIgnoreCase))
            //    return Ok(result);

            //return BadRequest(result);
            return Ok();
        }
    }
}