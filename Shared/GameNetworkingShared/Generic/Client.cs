using GameNetworkingShared.Protocols;

namespace GameNetworkingShared.Generic
{
    public abstract class Client
    {
        public abstract TCP Tcp { get; protected set; }

        //public abstract UDP Udp { get; set; }

        public abstract void Start();
    }
}
