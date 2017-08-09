using System;
using System.Text;
using CommonClassLibrary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SimpleTest.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopServer();


            var queueName = "queuetestapp01";

            var channel = CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnReceived;


            channel.BasicConsume(queueName,
                true,
                consumer);

            Common.Show("Waiting ...");
            Console.ReadLine();
        }

        private static void OnReceived(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Common.Show($"Received '{message}'");
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
    }
}