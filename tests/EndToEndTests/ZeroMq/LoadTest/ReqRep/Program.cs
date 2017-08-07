using System;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;

namespace ReqRep
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ScreenTop("Server");

        

            var factory = new ZeroMqMessageQueueFactory();
            var server = factory.CreateInboundQueue("LoadTestRepReq", MessagePattern.RequestResponse);


            server.Listen(message =>
            {
                server.Send(new Message
                {
                    Body = "Hi back",
                    ResponseAddress = message.ResponseAddress,
                    ResponseKey = message.ResponseKey
                });
            });
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