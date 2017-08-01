using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SimpleTest.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueName = "queuetestapp01";
            //var exchangeName = "exchangetestapp01";


            ScreenTop("Server");
            var model = CreateModel();

            var consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(queueName, false, consumer);

            while (true)
            {
                //Get next message
                var deliveryArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //Serialize message
                var message = Encoding.Unicode.GetString(deliveryArgs.Body);

                Console.WriteLine("Message Recieved - {0}", message);
                model.BasicAck(deliveryArgs.DeliveryTag, false);
            }


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
            model.BasicQos(0, 1, false);
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