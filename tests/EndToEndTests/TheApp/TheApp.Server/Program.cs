using System;
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
            var server = factory.CreateInboundQueue(new MessageQueueConfig("create-customer-sub", MessagePattern.PublishSubscribe)
            {
                SubscribeKey = "create"
            });

            CommonClassLibrary.Common.Show("Waiting for message ...");

            server.Listen(message => { CommonClassLibrary.Common.Show($"{message.BodyAs<string>()}"); });

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