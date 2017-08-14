using System;
using System.Text;
using System.Threading;
using Autofac;
using Messaging.Infrastructure.Messaging;
using TheApp.Common;

namespace TheApp.Server
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            var container = ConfigureDependencies();


            CommonClassLibrary.Common.ScreenTopServer();

            var factory = container.Resolve<MessageQueueFactory>();
            var serverReceive = factory.CreateInboundQueue(new MessageQueueConfig("create-customer-sub", MessagePattern.PublishSubscribe)
            {
                SubscribeKey = "create"
            });

            CommonClassLibrary.Common.Show("Waiting for message ...");

            serverReceive.Listen(message =>Process(message, factory));

            Console.ReadKey();
        }

        public static void Process(Message message, MessageQueueFactory factory)
        {
            CommonClassLibrary.Common.Show($"Received {message.BodyAs<string>()}");

            var key = Encoding.UTF8.GetString(message.ResponseKey);

            CommonClassLibrary.Common.Show($"Start to process ...");
            var serverSend = factory.CreateOutboundQueue("customer-created", MessagePattern.PublishSubscribe);

            //The Process ...
            Thread.Sleep(3000);

            CommonClassLibrary.Common.Show($"End of process ...");

         

            serverSend.Send(new Message()
            {
                Body = $"{message.BodyAs<string>()} Result"
            }, key);

            CommonClassLibrary.Common.Show($"Result for {key} sended.");
        }


        private static IContainer ConfigureDependencies()
        {
            // Register default dependencies in the application container.
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            return builder.Build();
        }
    }
}