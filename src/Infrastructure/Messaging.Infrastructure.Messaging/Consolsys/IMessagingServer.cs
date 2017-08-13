namespace Messaging.Infrastructure.Messaging.Consolsys
{
    public interface IMessagingServer
    {
        void Send<TMessage>(TMessage message);
        TResult Receive<TResult>();
    }
}