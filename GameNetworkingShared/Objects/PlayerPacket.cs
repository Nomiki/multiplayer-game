using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(517118269)]
    public class PlayerPacket : IPacketSerializable
    {
        [Ordered]
        public int Id { get; set; }

        [Ordered]
        public int ShipModelId { get; set; }

        [Ordered]
        public string Username { get; set; } = string.Empty;

        [Ordered]
        public PlayerPosition Position { get; set; }
    }
}
