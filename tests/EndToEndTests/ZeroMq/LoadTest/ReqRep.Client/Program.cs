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
                //Console.WriteLine(serverMessage);
            }
            stopWatch.Stop();
            var ts = stopWatch.Elapsed;
            var elapsedTime = $"Minute:Second:Millisecond {ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00} For {messageCount} Messages";
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadKey();
        }
    }
}