using System;
using System.Diagnostics;
using CommonClassLibrary;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;

namespace ReqRep.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           Common.ScreenTopClient();

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
            Common.Show($"RunTime Minute:Second:Millisecond " +
                 $"{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00} " +
                 $"For {messageCount} Messages");
            Console.ReadKey();
        }

    }
}