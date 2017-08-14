using NetMQ;
using NetMQ.Sockets;

namespace Messaging.Infrastructure.Messaging.ZeroMq.Consolsys
{
    public class ZeroMqMessagingIntermediary
    {
        public void Start()
        {
            using (var xpubSocket = new XPublisherSocket("@tcp://127.0.0.1:1234"))
            using (var xsubSocket = new XSubscriberSocket("@tcp://127.0.0.1:5678"))
            {
                var proxy = new Proxy(xsubSocket, xpubSocket);
                proxy.Start();
            }
        }
    }
}