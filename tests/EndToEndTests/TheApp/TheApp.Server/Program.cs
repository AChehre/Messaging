using System;
using System.Threading;
using Autofac;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq.Consolsys;
using TheApp.Common;

namespace TheApp.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ConfigureDependencies();

            CommonClassLibrary.Common.ScreenTopServer();

            var factory = container.Resolve<IMessageQueueFactory>();

            var messagingServer = new ZeroMqMessagingServer(factory);


            CommonClassLibrary.Common.Show("Waiting for message ...");

            var request = messagingServer.ReceiveRequest<string>();

            CommonClassLibrary.Common.Show($"Received {request}");


            CommonClassLibrary.Common.Show($"Start to process ...");
            //The Process ...
            Thread.Sleep(3000);

            var result = $"{request} Result";
            CommonClassLibrary.Common.Show($"Result for {result} replied.");
            messagingServer.SendResult(result);

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