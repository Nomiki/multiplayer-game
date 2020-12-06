using GameNetworkingShared.Logging;
using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameNetworkingShared.Objects
{
    public static class ObjectToIdFactory
    {
        private static Dictionary<int, Type> ObjectTypeByIds { get; set; }
            = new Dictionary<int, Type>();

        static ObjectToIdFactory()
        {
            Type @interface = typeof(IPacketSerializable);
            var assembly = @interface.Assembly;
            ObjectTypeByIds = assembly.GetTypes()
                .Where(t => @interface.IsAssignableFrom(t))
                .Select(t => new
                {
                    type = t,
                    id = t.GetCustomAttribute<PacketTypeIdAttribute>()?.ID
                })
                .Where(a => a.id.HasValue)
                .ToDictionary(a => a.id.Value, a => a.type);
        }

        public static Type TypeById(this int id)
        {
            if (ObjectTypeByIds.ContainsKey(id))
            {
                return ObjectTypeByIds[id];
            }

            return null;
        }
    }
}
