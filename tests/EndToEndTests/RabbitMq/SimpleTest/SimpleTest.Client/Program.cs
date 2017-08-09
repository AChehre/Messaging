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


            var model = CreateModel();
            var exchangeName = "exchangetestapp01";

            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = false;

            var message = Encoding.UTF8.GetBytes("Hello Ahmad!");
            var key = "testapp01";

            model.BasicPublish(exchangeName, key, basicProperties, message);

            Common.Show($"Message {Encoding.UTF8.GetString(message)} published.");


            Console.ReadKey();
        }

        private static void CreateExchangeAndQueue(IModel model, string queueName, string exchangeName)
        {
            model.QueueDeclare(queueName, true, false, false, null);

            Common.Show($"Queue {queueName} Created.");

            model.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);

            Common.Show($"Exchange {exchangeName} Created.");

            model.QueueBind(queueName, exchangeName, "testapp01");

            Common.Show($"Queue {queueName} and exchange {exchangeName} binded.");
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