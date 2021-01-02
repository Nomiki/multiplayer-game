using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(61028841)]
    public class PlayerMovement : IPacketSerializable
    {
        [Ordered]
        public bool Up { get; set; }

        [Ordered]
        public bool Down { get; set; }

        [Ordered]
        public bool Left { get; set; }
        
        [Ordered]
        public bool Right { get; set; }

        [Ordered]
        public float Angle { get; set; }

        public PlayerMovement()
        {
            // Emtpy Ctor;
        }
    }
}
