using System;
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
            var client = factory.CreateOutboundQueue("create-customer-pub", MessagePattern.PublishSubscribe);

            var message = new Message
            {
                Body = "Console Customer"
            };

            client.Send(message, "create");

            CommonClassLibrary.Common.Show($"Message {message.Body} Sended!");

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