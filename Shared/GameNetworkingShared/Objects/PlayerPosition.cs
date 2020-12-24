using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(452160566)]
    public class PlayerPosition : IPacketSerializable
    {
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

        public PlayerPosition(float x, float y, float z = 0, float angle = 0)
        {
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
        }
    }
}
