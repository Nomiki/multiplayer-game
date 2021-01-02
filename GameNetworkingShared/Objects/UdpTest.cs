using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(688662409)]
    public class UdpTest : IPacketSerializable
    {
        [Ordered]
        public string Message { get; set; }
    }
}
