using GameNetworkingShared.Packet;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(414166372)]
    public class WelcomeMessage : IPacketSerializable
    {
        [Ordered]
        public string Message { get; set; } = string.Empty;

        [Ordered]
        public string Username { get; set; } = string.Empty;
    }
}
