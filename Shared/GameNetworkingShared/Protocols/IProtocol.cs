using GameNetworkingShared.Packets;
using System.Net;

namespace GameNetworkingShared.Protocols
{
    public interface IProtocol
    {
        void SendData(PacketBase packet, IPEndPoint endPoint = null);
    }
}
