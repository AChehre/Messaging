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


            var channel = CreateModel();
            channel.ExchangeDeclare("logs", "fanout");

            var queueName = "logsQueue";


            CreateAndBindQueue(channel, "logs", "logsQueue", "logs");


            channel.QueueBind(queueName,
                "logs",
                "logs");

            Common.Show(" [*] Waiting for logs.");
#pragma warning disable 0618
            var consumer = new QueueingBasicConsumer(channel);
#pragma warning restore 0618
            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += (model, ea) =>
            //{

            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);
            //    Console.WriteLine(" [x] {0}", message);
            //};

            //channel.BasicConsume(queueName,
            //    true,
            //    consumer);

            channel.BasicConsume("logsQueue",
                true,
                consumer);

            var ea = consumer.Queue.Dequeue();

            var message = Encoding.UTF8.GetString(ea.Body);
            Console.WriteLine(" [x] {0}", message);


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


        protected static void CreateAndBindQueue(IModel channel, string exchangeName, string queueName,
            string routingKey)
        {
            channel.QueueDeclare(queueName, false, false, true, null);
            channel.QueueBind(queueName, exchangeName, routingKey);
        }
    }
}