using System;
using System.Text;
using System.Threading;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.PubSub.ConsoleAppClient
{
    class Program
    {
        static void Main(string[] args)
        {
          

            CustomerCreatedResponse customerCreatedResponse = null;
            var messageQueueFactory = new ZeroMqMessageQueueFactory();


            var queue = messageQueueFactory.CreateOutboundQueue("customer-with-pubsub", MessagePattern.PublishSubscribe);
            for (int i = 0; i < 5; i++)
            {

                var createCustomerRequest = new CreateCustomerRequest(0, $"ahmad {i}");

                Console.WriteLine($"-----------{i}-----------");
                var key = Guid.NewGuid().ToString();
                var answerqueue =
                    messageQueueFactory.CreateInboundQueue(
                        new MessageQueueConfig("customer-with-pubsub-answer", MessagePattern.PublishSubscribe)
                        {
                            SubscribeKey = key
                        });


                Console.WriteLine(key);
                
                queue.Send(new Message
                {
                    Body = createCustomerRequest,
                    ResponseKey = Encoding.Unicode.GetBytes(key)
                }, "customer-with-pubsub");

                Console.WriteLine("message Sended.");
               
                answerqueue.Received(r => customerCreatedResponse = r.BodyAs<CustomerCreatedResponse>());


                customerCreatedResponse.ShowOnConsole();

              
            }



            Console.ReadKey();
        }
    }
}