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


#pragma warning disable 0618
        private readonly QueueingBasicConsumer consumer;
#pragma warning restore 0618


        private readonly string replyQueueName;

        public RPCClient()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;

#pragma warning disable 0618
            consumer = new QueueingBasicConsumer(channel);
#pragma warning restore 0618

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
            channel.BasicPublish("", "rpc_queue", props,
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