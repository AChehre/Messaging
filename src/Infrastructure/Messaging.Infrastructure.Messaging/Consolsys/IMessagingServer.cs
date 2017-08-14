namespace Messaging.Infrastructure.Messaging.Consolsys
{
    public interface IMessagingServer
    {
        void SendResult<TMessage>(TMessage message);
        TResult ReceiveRequest<TResult>();
    }
}