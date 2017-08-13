using System;
using CommonClassLibrary;
using Tests.ZeroMq.CommandQuery;

namespace ConsolsysApp.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopClient();
            var client = new MessagingClient();


            var command = new CreateCustomerRequest(1, "Ahmad");

            client.Send(command);

            var result = client.Receive();

            Console.WriteLine(result);
        }
    }


   
}