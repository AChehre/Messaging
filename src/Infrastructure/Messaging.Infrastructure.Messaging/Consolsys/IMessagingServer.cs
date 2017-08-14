namespace Messaging.Infrastructure.Messaging.Consolsys
{
    public interface IMessagingServer
    {
        TResult ReceiveRequest<TResult>();
        void SendResult<TMessage>(TMessage message);
      
    }
}