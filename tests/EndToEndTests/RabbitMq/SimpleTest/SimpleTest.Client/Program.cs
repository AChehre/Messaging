using System;
using System.Text;
using CommonClassLibrary;
using RabbitMQ.Client;

namespace SimpleTest.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopClient();

            var channel = CreateModel();

            channel.ExchangeDeclare("logs", "fanout");


            CreateAndBindQueue(channel, "logs", "logsQueue", "logs");

            var message = "Hi ...";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("logs",
                "logs",
                null,
                body);
            Common.Show($" [x] Sent {message}");


            Console.ReadKey();
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

            var model = connection.CreateModel();

            return model;
        }

        protected static void CreateAndBindQueue(IModel channel, string exchangeName, string queueName, string routingKey)
        {
            channel.QueueDeclare(queueName, false, false, true, null);
            channel.QueueBind(queueName, exchangeName, routingKey);
        }
    }
}