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

            ScreenTop("Server");

            Console.WriteLine("Listening ...");
            var factoryAsync = new ZeroMqMessageQueueFactoryAsync();
            var reqQueue = factoryAsync.CreateInboundQueue("mix-customer", MessagePattern.RequestResponse);
            var factory = new ZeroMqMessageQueueFactory();
            var pubQueue = factory.CreateOutboundQueue("mix-publish", MessagePattern.PublishSubscribe);

            reqQueue.Listen(message => { Process(reqQueue, pubQueue, message); });

         
            //Thread.Sleep(50000);
            Console.WriteLine();
        }

        private static void Process(IMessageQueue reqQueue, IMessageQueue pubQueue, Message message)
        {
            var publisherKey = Guid.NewGuid().ToString();

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

        private static void ScreenTop(string title)
        {
            var dashes = new string('-', title.Length + 20);

            Console.WriteLine(dashes);
            Console.WriteLine($"|{new string(' ', 9)}{title}{new string(' ', 9)}|");
            Console.WriteLine(dashes);
        }
    }
}