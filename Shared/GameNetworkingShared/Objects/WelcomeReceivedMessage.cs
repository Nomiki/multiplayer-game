using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(16786749)]
    public class WelcomeReceivedMessage : IPacketSerializable
    {
        [Ordered]
        public string Username { get; set; } = string.Empty;

        [Ordered]
        public int ClientId { get; set; }
    }
}
