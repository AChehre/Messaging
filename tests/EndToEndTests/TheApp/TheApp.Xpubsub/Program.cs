using CommonClassLibrary;
using Messaging.Infrastructure.Messaging.ZeroMq.Consolsys;

namespace TheApp.Xpubsub
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Common.ScreenTop("Intermediary");
            var zeroMqMessagingIntermediary = new ZeroMqMessagingIntermediary();
            zeroMqMessagingIntermediary.Start();
        }
    }
}