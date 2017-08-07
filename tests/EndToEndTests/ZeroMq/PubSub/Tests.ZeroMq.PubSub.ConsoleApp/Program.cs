using System;
using System.Text;
using System.Threading;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.PubSub.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopServer();

            var factory = new ZeroMqMessageQueueFactory();
            var sub = factory.CreateInboundQueue(new MessageQueueConfig("customer-with-pubsub", MessagePattern.PublishSubscribe)
            {
                SubscribeKey = "customer-with-pubsub"
            });


            var answerServer = factory.CreateOutboundQueue("customer-with-pubsub-answer", MessagePattern.PublishSubscribe);


          sub.Listen(message=>GetValue(answerServer, message));
        }

        private static void GetValue(IMessageQueue answerServer, Message message)
        {
            Common.Show("*****************");
            Common.Show("message received...");
            message.BodyAs<CreateCustomerRequest>().ShowOnConsole();
            var id = 1000;
            Thread.Sleep(100);
            var customerCreatedResponse = new CustomerCreatedResponse
            {
                Id = id //Created customer Id
            };

            // Process
            var replyMessage = new Message()
            {
                Body = customerCreatedResponse,
            };
            var key = Encoding.Unicode.GetString(message.ResponseKey);
            answerServer.Send(replyMessage, key);
            Common.Show($"message send by key:{Encoding.Unicode.GetString(message.ResponseKey)}");
            Common.Show("+++++++++++++++");
        }
    }
}