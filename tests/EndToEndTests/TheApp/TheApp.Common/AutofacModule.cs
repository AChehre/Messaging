using Autofac;
using Messaging.Infrastructure.Messaging;
using Messaging.Infrastructure.Messaging.ZeroMq;

namespace TheApp.Common
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ZeroMqMessageQueueFactory>().As<IMessageQueueFactory>();
        }

    }
}