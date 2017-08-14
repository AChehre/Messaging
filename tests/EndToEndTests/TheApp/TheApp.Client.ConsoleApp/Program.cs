using System;
using Autofac;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq.Consolsys;
using TheApp.Common;

namespace TheApp.Client.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ConfigureDependencies();
            var factory = container.Resolve<IMessageQueueFactory>();

            CommonClassLibrary.Common.ScreenTopClient();


            var messagingClient = new ZeroMqMessagingClient(factory);

            var message = "Console Customer";
            CommonClassLibrary.Common.Show($"{message} requested.");

            messagingClient.SendRequest(message);

            var result = messagingClient.ReceiveResult<string>();


            CommonClassLibrary.Common.Show($"Received {result}");


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