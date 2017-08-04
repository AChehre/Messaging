namespace Messaging.Infrastructure.Messaging.RabbitMq
{
    public class RabbitMqConfig
    {
        public readonly RabbitMqBinding Bindings;
        public readonly string HostName;
        public readonly string Password;
        public readonly string UserName;


        public RabbitMqConfig(string hostName, string userName, string password, RabbitMqBinding bindings)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
            Bindings = bindings;
        }

        public RabbitMqConfig(string hostName, string userName, string password)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;
        }

        public bool CreateExchange { get; set; } = false;
        public bool CreateQueue { get; set; } = false;
    }
}