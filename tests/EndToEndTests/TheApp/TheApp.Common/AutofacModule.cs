using Autofac;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.Consolsys;
using Messaging.Infrastructure.Messaging.RabbitMq;
using Messaging.Infrastructure.Messaging.RabbitMq.Consolsys;
using Messaging.Infrastructure.Messaging.ZeroMq;
using Messaging.Infrastructure.Messaging.ZeroMq.Consolsys;

namespace TheApp.Common
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ZeroMqMessageQueueFactory>().As<IMessageQueueFactory>();
            //builder.RegisterType<ZeroMqMessagingClient>().As<IMessagingClient>();
            //builder.RegisterType<ZeroMqMessagingServer>().As<IMessagingServer>();


            builder.RegisterType<RabbitMqMessageQueueFactory>().As<IMessageQueueFactory>();
            builder.RegisterType<RabbitMqMessagingClient>().As<IMessagingClient>();
            builder.RegisterType<RabbitMqMessagingServer>().As<IMessagingServer>();
        }
    }
}