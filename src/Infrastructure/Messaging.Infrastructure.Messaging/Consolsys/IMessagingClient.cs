namespace Messaging.Infrastructure.Messaging.Consolsys
{
    public interface IMessagingClient
    {
        void Send<TMessage>(TMessage message);
        TResult Receive<TResult>();
    }
}