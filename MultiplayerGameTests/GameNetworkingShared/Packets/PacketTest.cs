﻿using GameNetworkingShared.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiplayerGameTests.Infra;

namespace MultiplayerGameTests.GameNetworkingShared.Packets
{
    [TestClass]
    public class PacketTest
    {
        [TestMethod]
        public void TestPacket_ObjectOfFloats_ShouldConvertBackSuccessfully()
        {
            VectorThreeMock obj = new VectorThreeMock()
            {
                First = 3.22f,
                Second = 4.222f,
                Third = -5.4f
            };

            VectorThreeMock copy = DoPacketRoundTripMock(obj);

            copy.First.ShouldEqual(obj.First);
            copy.Second.ShouldEqual(obj.Second);
            copy.Third.ShouldEqual(obj.Third);
        }

        [TestMethod]
        public void TestPacket_ClassWithStringsAndNumbers_ShouldConvertBackSuccessfully()
        {
            ClassWithStringAndNumbers obj = new ClassWithStringAndNumbers()
            {
                FirstString = "Hello World!",
                Number = -1235,
                SecondString = "Welcome to my test",
                BooleanValue = true,
            };

            ClassWithStringAndNumbers response = DoPacketRoundTripMock(obj);

            response.FirstString.ShouldEqual(obj.FirstString);
            response.SecondString.ShouldEqual(obj.SecondString);
            response.Number.ShouldEqual(obj.Number);
            response.BooleanValue.ShouldEqual(obj.BooleanValue);
        }

        [TestMethod]
        public void TestPacket_ClassWithStringsAndNumbers_WithEmptyString_ShouldConvertBackSuccessfully()
        {
            ClassWithStringAndNumbers obj = new ClassWithStringAndNumbers()
            {
                FirstString = "",
                Number = 666,
                SecondString = "",
                BooleanValue = true,
            };

            ClassWithStringAndNumbers response = DoPacketRoundTripMock(obj);

            response.FirstString.ShouldEqual(obj.FirstString);
            response.SecondString.ShouldEqual(obj.SecondString);
            response.Number.ShouldEqual(obj.Number);
            response.BooleanValue.ShouldEqual(obj.BooleanValue);
        }

        [TestMethod]
        public void TestPacket_ClassAllTypes_ShouldConvertBackSuccessfully()
        {
            ClassWithAllTypes obj = new ClassWithAllTypes()
            {
                ByteProperty = (byte)45,
                BooleanProperty = false,
                FloatProperty = 2334.6454f,
                IntegerProperty = 54471,
                LongIntProperty = 43672942002023,
                ShortProperty = 3123,
                StringProperty = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            };

            ClassWithAllTypes response = DoPacketRoundTripMock(obj);

            response.ByteProperty.ShouldEqual(obj.ByteProperty);
            response.BooleanProperty.ShouldEqual(obj.BooleanProperty);
            response.FloatProperty.ShouldEqual(obj.FloatProperty);
            response.IntegerProperty.ShouldEqual(obj.IntegerProperty);
            response.LongIntProperty.ShouldEqual(obj.LongIntProperty);
            response.ShortProperty.ShouldEqual(obj.ShortProperty);
            response.StringProperty.ShouldEqual(obj.StringProperty);
        }

        private static T DoPacketRoundTripMock<T>(T obj) where T : IPacketSerializable
        {
            T copy = default(T);

            using (Packet p = new Packet())
            {
                p.WriteObj<T>(obj);

                using (Packet p2 = new Packet())
                {
                    p2.SetBytes(p.ToArray());
                    copy = p2.ReadObj<T>();
                }
            }

            return copy;
        }

        [PacketTypeId(12334433)]
        private class VectorThreeMock : IPacketSerializable
        {
            [Ordered]
            public float First { get; set; }

            [Ordered]
            public float Second { get; set; }

            [Ordered]
            public float Third { get; set; }
        }

        [PacketTypeId(77665777)]
        private class ClassWithStringAndNumbers : IPacketSerializable
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
        private class ClassWithAllTypes : IPacketSerializable
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
}
