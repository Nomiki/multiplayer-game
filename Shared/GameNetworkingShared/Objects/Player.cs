using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(517118269)]
    public class Player : IPacketSerializable
    {
        [Ordered]
        public int Id { get; set; }

        [Ordered]
        public string Username { get; set; } = string.Empty;

        [Ordered]
        public PlayerPosition Position { get; set; }
    }
}
