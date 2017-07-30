using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.Mix.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Listening ...");
            var factoryAsync = new ZeroMqMessageQueueFactoryAsync();
            var reqQueue = factoryAsync.CreateInboundQueue("mix-customer", MessagePattern.RequestResponse);
            var factory = new ZeroMqMessageQueueFactory();
            var pubQueue = factory.CreateOutboundQueue("mix-publish", MessagePattern.PublishSubscribe);

            reqQueue.Listen(message => { Process(reqQueue, pubQueue, message); });

            //Process(null, null);
            Thread.Sleep(50000);
            Console.WriteLine();
        }

        private static void Process(IMessageQueue reqQueue, IMessageQueue pubQueue, Message message)
        {
            var publisherKey = "0d7b8247-d74a-4060-b0bf-a006db9d182c";//Guid.NewGuid().ToString();

            var repQueue = reqQueue.GetReplyQueue(message);

            repQueue.Send(new Message
            {
                Body = publisherKey,
                ResponseAddress = message.ResponseAddress,
                ResponseKey = message.ResponseKey
            });

            var createCustomerRequest = message.BodyAs<CreateCustomerRequest>();

            createCustomerRequest.ShowOnConsole();

            
            Console.WriteLine("sending...");

           


            var replyMessage = new Message
            {
                Body = new CustomerCreatedResponse
                {
                    Id = createCustomerRequest.Id
                    //Id = 1
                }
            };


            Thread.Sleep(100);
            pubQueue.Send(replyMessage, publisherKey);
        }
    }
}