using System;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;

namespace ReqRep
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var server = new ZeroMqMessageQueue();
            server.InitializeInbound("LoadTestRepReq", MessagePattern.RequestResponse);

            while (true)
                server.Listen(message =>
                {
                    //Console.WriteLine(message.BodyAs<string>());
                    server.Send(new Message
                    {
                        Body = "Hi back",
                        ResponseAddress = message.ResponseAddress,
                        ResponseKey = message.ResponseKey
                    });
                });
        }
    }
}