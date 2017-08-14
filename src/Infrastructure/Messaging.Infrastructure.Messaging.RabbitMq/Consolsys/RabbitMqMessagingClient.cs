using Messaging.Infrastructure.Messaging.Consolsys;

namespace Messaging.Infrastructure.Messaging.RabbitMq.Consolsys
{
    public class RabbitMqMessagingClient : IMessagingClient
    {
        private readonly IMessageQueue _client;


        public RabbitMqMessagingClient(IMessageQueueFactory factory)
        {
            _client = factory.CreateOutboundQueue("", MessagePattern.FireAndForget);
        }

        public void SendRequest<TMessage>(TMessage message)
        {
            _client.Send(new Message {Body = message}, "rpc_queue");
        }

        public TResult ReceiveResult<TResult>()
        {
            var result = default(TResult);
            _client.Received(message => { result = message.BodyAs<TResult>(); });
            return result;
        }
    }
}