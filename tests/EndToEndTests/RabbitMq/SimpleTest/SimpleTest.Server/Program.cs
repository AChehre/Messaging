using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SimpleTest.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var queueName = "queuetestapp01";
            //var exchangeName = "exchangetestapp01";


            ScreenTop("Server");
            var channel = CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceived;

            var a = channel.BasicConsume(queueName,
                true,
                consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void OnReceived(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        }

        private static IModel CreateModel()
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };


            var connection = factory.CreateConnection();
            Show("Connection Created.");
            var model = connection.CreateModel();
            Show("Model Created;");
            //model.BasicQos(0, 1, false);
            return model;
        }


        private static void Show(string message)
        {
            Console.WriteLine(message);
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