using System;
using System.Diagnostics;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;

namespace ReqRep.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ScreenTop("Client");

            var client = new ZeroMqMessageQueue();
            client.InitializeOutbound("LoadTestRepReq", MessagePattern.RequestResponse);

            var messageCount = 1000;

            var stopWatch = new Stopwatch();
            stopWatch.Start();


            for (var i = 0; i < messageCount; i++)
            {

                var responseQueue = client.GetResponseQueue();

                client.Send(new Message
                {
                    Body = $"Hi {i}",
                    ResponseAddress = responseQueue.Address
                });

                var serverMessage = "";
                responseQueue.Received(r => serverMessage = r.BodyAs<string>());
            }

            stopWatch.Stop();
            var ts = stopWatch.Elapsed;
            Show($"RunTime Minute:Second:Millisecond " +
                 $"{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00} " +
                 $"For {messageCount} Messages");
            Console.ReadKey();
        }

        private static void Show(string message)
        {
            Console.WriteLine($"{message}\n");
        }

        private static void ScreenTop(string title)
        {
            var dashes = new string('-', title.Length + 20);

            Console.WriteLine(dashes);
            Console.WriteLine($"|{new string(' ', 9)}{title}{new string(' ', 9)}|");
            Console.WriteLine(dashes);
        }
    }
}