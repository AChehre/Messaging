using System;
using System.Threading;
using Autofac;
using Messaging.Infrastructure.Messaging.Consolsys;
using TheApp.Common;

namespace TheApp.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ConfigureDependencies();

            CommonClassLibrary.Common.ScreenTopServer();

            var messagingServer = container.Resolve<IMessagingServer>();

            CommonClassLibrary.Common.Show("Waiting for message ...");

            messagingServer.ReceiveRequest<string>(request => { Process(request, messagingServer); });
            Console.ReadKey();
        }

        private static void Process(string request, IMessagingServer messagingServer)
        {
            CommonClassLibrary.Common.Show($"Received {request}");
            CommonClassLibrary.Common.Show($"Start to process ...");
            //The Process ...
            Thread.Sleep(3000);

            var result = $"{request} Result";
            CommonClassLibrary.Common.Show($"Result for {result} replied.");
            messagingServer.SendResult(result);
        }


        private static IContainer ConfigureDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            return builder.Build();
        }
    }
}