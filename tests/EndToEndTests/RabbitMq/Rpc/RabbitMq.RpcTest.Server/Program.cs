﻿using System;
using System.Text;
using System.Threading;
using CommonClassLibrary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.RpcTest.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopServer();
            var factory = new ConnectionFactory {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("rpc_queue", false,
                    false, false, null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume("rpc_queue",
                    false, consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        Common.Show($"Message received {message}");

                        Thread.Sleep(2000);

                        var n = int.Parse(message);
                        Console.WriteLine(" [.] fib({0})", message);
                        response = fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish("", props.ReplyTo,
                            replyProps, responseBytes);
                        channel.BasicAck(ea.DeliveryTag,
                            false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

            int fib(int n)
            {
                if (n == 0 || n == 1)
                    return n;

                return fib(n - 1) + fib(n - 2);
            }

            Console.ReadKey();
        }
    }
}