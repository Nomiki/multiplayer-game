using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerGameTests.Infra
{
    [PacketTypeId(12334433)]
    public class VectorThreeMock : IPacketSerializable
    {
        [Ordered]
        public float First { get; set; }

        [Ordered]
        public float Second { get; set; }

        [Ordered]
        public float Third { get; set; }
    }

    [PacketTypeId(77665777)]
    public class ClassWithStringAndNumbers : IPacketSerializable
    {
        [Ordered]
        public string FirstString { get; set; }

        [Ordered]
        public int Number { get; set; }

        [Ordered]
        public string SecondString { get; set; }

        [Ordered]
        public bool BooleanValue { get; set; }
    }

    [PacketTypeId(3346712)]
    public class ClassWithAllTypes : IPacketSerializable
    {
        [Ordered]
        public bool BooleanProperty { get; set; }

        [Ordered]
        public byte ByteProperty { get; set; }

        [Ordered]
        public short ShortProperty { get; set; }

        [Ordered]
        public int IntegerProperty { get; set; }

        [Ordered]
        public long LongIntProperty { get; set; }

        [Ordered]
        public float FloatProperty { get; set; }

        [Ordered]
        public string StringProperty { get; set; }
    }
}
