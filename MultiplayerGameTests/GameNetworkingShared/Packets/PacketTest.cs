using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiplayerGameTests.Infra;
using System.Reflection;

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

        [TestMethod]
        public void TestPacket_ClassHasNestedIPacketSerializable_ShouldConvertBackSuccessfully()
        {
            PlayerPacket obj = new PlayerPacket()
            {
                Id = 3,
                Username = "username",
                Position = new PlayerPosition()
                {
                    Id = 3,
                    Angle = 143.4f,
                    X = -4f,
                    Y = -5f,
                    Z = 4f,
                },
            };

            PlayerPacket response = DoPacketRoundTripMock(obj);

            response.Id.ShouldEqual(obj.Id);
            response.Username.ShouldEqual(obj.Username);
            response.Position.Angle.ShouldEqual(obj.Position.Angle);
            response.Position.X.ShouldEqual(obj.Position.X);
            response.Position.Y.ShouldEqual(obj.Position.Y);
            response.Position.Z.ShouldEqual(obj.Position.Z);
        }

        private static T DoPacketRoundTripMock<T>(T obj) where T : IPacketSerializable
        {
            T copy = default(T);

            using (Packet p = new Packet())
            {
                p.WriteObj<T>(obj);

                using (Packet p2 = new Packet(p.ToArray()))
                {
                    int objId = p2.ReadInt();
                    int expectedObjId =
                        typeof(T).GetCustomAttribute<PacketTypeIdAttribute>(false).ID;

                    objId.ShouldEqual(expectedObjId);
                    copy = p2.ReadObj<T>();
                }
            }

            return copy;
        }
    }
}
