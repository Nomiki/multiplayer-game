using GameNetworkingShared.Protocols;
using System.Net;
using UnityEngine;

namespace GameNetworkingShared.Generic
{
    public abstract class Client
    {
        public abstract TCP Tcp { get; protected set; }

        public EndPoint TcpEndpoint => Tcp?.Socket?.Client.RemoteEndPoint;

        public abstract void Start();

        public virtual void Disconnect()
        {
            Tcp?.Disconnect();
        }
    }
}
