using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(44021576)]
    public class PlayerDisconnectedPacket : IPacketSerializable
    {
        [Ordered]
        public int Id { get; set; }
    }
}
