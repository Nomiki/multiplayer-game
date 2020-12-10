using GameNetworkingShared.Protocols;
using System.Net;

namespace GameNetworkingShared.Generic
{
    public abstract class Client
    {
        public abstract TCP Tcp { get; protected set; }

        public EndPoint TcpEndpoint => Tcp.Socket.Client.RemoteEndPoint;

        public abstract void Start();
    }
}
