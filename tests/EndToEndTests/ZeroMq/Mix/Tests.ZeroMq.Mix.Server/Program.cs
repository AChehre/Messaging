using System;
using System.Threading;
using System.Threading.Tasks;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Tests.ZeroMq.CommandQuery;

namespace Tests.ZeroMq.Mix.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTop("Server");

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
            Common.Show(new string('-', 20));

            var publisherKey = Guid.NewGuid().ToString();

            var repQueue = reqQueue.GetReplyQueue(message);


            repQueue.Send(new Message
            {
                Body = publisherKey,
                ResponseAddress = message.ResponseAddress,
                ResponseKey = message.ResponseKey
            });


            Task.Factory.StartNew(() =>
            {
                var delayTime = message.BodyAs<CreateCustomerRequest>().Id / 10 * 1000;

                Thread.Sleep(delayTime);
                Common.Show($" Proccessing on {message.BodyAs<CreateCustomerRequest>()}");
                Common.Show($"Sending by {publisherKey}");
                var replyMessage = new Message
                {
                    Body = new CustomerCreatedResponse
                    {
                        Id = message.BodyAs<CreateCustomerRequest>().Id
                    }
                };


                pubQueue.Send(replyMessage, publisherKey);
                Common.Show("Sended!");
            }, TaskCreationOptions.LongRunning);
        }
    }
}