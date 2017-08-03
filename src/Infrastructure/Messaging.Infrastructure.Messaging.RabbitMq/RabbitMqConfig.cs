namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqConfig
    {
        public readonly string HostName;
        public readonly string Password;
        public readonly string UserName;

        public RabbitMqConfig(string hostName, string userName, string password)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
        }
    }
}