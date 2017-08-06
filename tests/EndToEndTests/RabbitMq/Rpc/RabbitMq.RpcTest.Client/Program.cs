using System;
using System.Text;
using CommonClassLibrary;
using RabbitMQ.Client;

namespace RabbitMq.RpcTest.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTopClient();

            var rpcClient = new RPCClient();

            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");
            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();
            Console.ReadKey();
        }
    }

    internal class RPCClient
    {
        private readonly IModel channel;
        private readonly IConnection connection;
        private readonly QueueingBasicConsumer consumer;
        private readonly string replyQueueName;

        public RPCClient()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(replyQueueName,
                true,
                consumer);
        }

        public string Call(string message)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("",
                "rpc_queue",
                props,
                messageBytes);

            while (true)
            {
                var ea = consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                    return Encoding.UTF8.GetString(ea.Body);
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }
}