namespace Messaging.Infrastructure.Messaging.Consolsys
{
    public interface IMessagingClient
    {
        void SendRequest<TMessage>(TMessage message);
        TResult ReceiveResult<TResult>();
    }
}