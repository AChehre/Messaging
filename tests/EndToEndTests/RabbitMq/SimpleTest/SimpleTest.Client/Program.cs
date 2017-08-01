using System;
using System.Text;
using RabbitMQ.Client;

namespace SimpleTest.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ScreenTop("Client");


            var model = CreateModel();
            //var queueName = "queuetestapp01";
            var exchangeName = "exchangetestapp01";

            //CreateExchangeAndQueue(model, queueName, exchangeName);

            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = false;

            var message = Encoding.UTF8.GetBytes("Hello 3 from app!");

            var key = "testapp01";
            model.BasicPublish(exchangeName, key, basicProperties, message);
            Show($"Message {Encoding.UTF8.GetString(message)} published.");





            Console.ReadKey();
        }

        private static void CreateExchangeAndQueue(IModel model, string queueName, string exchangeName)
        {
            model.QueueDeclare(queueName, true, false, false, null);

            Show($"Queue {queueName} Created.");

            model.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);

            Show($"Exchange {exchangeName} Created.");

            model.QueueBind(queueName, exchangeName, "testapp01");

            Show($"Queue {queueName} and exchange {exchangeName} binded.");
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