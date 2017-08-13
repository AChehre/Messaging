using System;
using CommonClassLibrary;
using NetMQ;
using NetMQ.Sockets;

namespace TheApp.Xpubsub
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTop("Intermediary");

            using (var xpubSocket = new XPublisherSocket("@tcp://127.0.0.1:1234"))
            using (var xsubSocket = new XSubscriberSocket("@tcp://127.0.0.1:5678"))
            {
                Common.Show("Intermediary started, and waiting for messages");

                // proxy messages between frontend / backend
                var proxy = new Proxy(xsubSocket, xpubSocket);

                // blocks indefinitely
                proxy.Start();
            }
            Console.ReadKey();
        }
    }
}