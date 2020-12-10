using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Protocols
{
    public interface IProtocol
    {
        void SendData(PacketBase packet);
    }
}
