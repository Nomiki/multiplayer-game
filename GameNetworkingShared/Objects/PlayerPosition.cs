using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(452160566)]
    public class PlayerPosition : IPacketSerializable
    {
        [Ordered]
        public int Id { get; set; }
        
        [Ordered]
        public float X { get; set; }

        [Ordered]
        public float Y { get; set; }

        [Ordered]
        public float Z { get; set; }

        [Ordered]
        public float Angle { get; set; }

        public PlayerPosition()
        {
            // Emtpy Ctor;
        }
    }
}
