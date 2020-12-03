﻿using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(414166372)]
    public class WelcomeMessage : IPacketSerializable
    {
        [Ordered]
        public string Message { get; set; } = string.Empty;

        [Ordered]
        public int ClientId { get; set; }
    }
}
