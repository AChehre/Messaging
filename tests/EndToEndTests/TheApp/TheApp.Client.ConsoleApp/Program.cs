using System;
using Autofac;
using Messaging.Infrastructure.Messaging.Consolsys;
using TheApp.Common;

namespace TheApp.Client.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ConfigureDependencies();

            CommonClassLibrary.Common.ScreenTopClient();

            var messagingClient = container.Resolve<IMessagingClient>();

            var message = "Console Customer";
            CommonClassLibrary.Common.Show($"{message} requested.");

            messagingClient.SendRequest(message);

            var result = messagingClient.ReceiveResult<string>();


            CommonClassLibrary.Common.Show($"Received {result}");


            Console.ReadKey();
        }


        private static IContainer ConfigureDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            return builder.Build();
        }
    }
}