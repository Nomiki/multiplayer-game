using System;

namespace GameNetworkingShared.Packet
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class PacketTypeIdAttribute : Attribute
    {
        public int ID { get; private set; }

        public PacketTypeIdAttribute(int id)
        {
            ID = id;
        }
    }
}
