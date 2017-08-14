using System;
using System.Text;
using Autofac;
using Messaging.Infrastructure.Messaging;
using TheApp.Common;

namespace TheApp.Client.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ConfigureDependencies();


            CommonClassLibrary.Common.ScreenTopClient();

            var factory = container.Resolve<MessageQueueFactory>();
            var clientSend = factory.CreateOutboundQueue("create-customer-pub", MessagePattern.PublishSubscribe);

            var key = Guid.NewGuid().ToString();

            var clientReceive = factory.CreateInboundQueue(
                new MessageQueueConfig("customer-created", MessagePattern.PublishSubscribe)
                {
                    SubscribeKey = key
                });


            var message = new Message
            {
                Body = "Console Customer",
                ResponseKey = Encoding.UTF8.GetBytes(key)
            };

            clientSend.Send(message, "create");

            CommonClassLibrary.Common.Show($"Message {message.Body} Sended!");

            CommonClassLibrary.Common.Show($"Wait for {key} ...");
            clientReceive.Received(result =>
            {
                CommonClassLibrary.Common.Show($"Received {result.BodyAs<string>()}");
            });


            Console.ReadKey();
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