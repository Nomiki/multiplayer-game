using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameNetworkingShared.Objects
{
    public static class ObjectsExtensions
    {
        private static Type @Interface => typeof(IPacketSerializable);
        private static Type[] SupportedTypes => new[] { typeof(bool),
                                                        typeof(byte),
                                                        typeof(short),
                                                        typeof(int),
                                                        typeof(long),
                                                        typeof(float),
                                                        typeof(string) };

        public static Dictionary<Type, PropertyInfo[]> PropertiesForType { get; set; }
            = new Dictionary<Type, PropertyInfo[]>();

        public static Dictionary<Type, int> TypeIdAttribute { get; set; }
            = new Dictionary<Type, int>();

        private static PropertyInfo[] GetOrderedProperties<T>()
        {
            Type typeofT = typeof(T);
            return GetOrderedProperties(typeofT);
        }

        public static PropertyInfo[] GetOrderedProperties(this Type typeofT)
        {
            if (PropertiesForType.ContainsKey(typeofT))
            {
                return PropertiesForType[typeofT];
            }

            PropertyInfo[] propertiesForT = typeofT.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(OrderedAttribute)))
                .Where(p => IsSupportedType(p.PropertyType))
                .OrderBy(p => p.GetCustomAttribute<OrderedAttribute>().Order)
                .ToArray();

            int packetTypeId = typeofT.GetCustomAttribute<PacketTypeIdAttribute>(false).ID;

            TypeIdAttribute[typeofT] = packetTypeId;
            PropertiesForType[typeofT] = propertiesForT;

            return propertiesForT;
        }

        private static bool IsSupportedType(Type t)
        {
            return SupportedTypes.Contains(t) || @Interface.IsAssignableFrom(t) && t != @Interface;
        }

        public static string ToJson<T>(this T obj) where T : IPacketSerializable
        {
            PropertyInfo[] props = GetOrderedProperties(obj.GetType());
            PropertyInfo lastProp = props.Last();
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            foreach (PropertyInfo prop in props)
            {
                sb.Append($"\"{prop.Name}\": ");
                if (prop.PropertyType == typeof(string))
                {
                    sb.Append($"\"{prop.GetValue(obj)}\"");
                }
                else if (@Interface.IsAssignableFrom(prop.PropertyType))
                {
                    IPacketSerializable value = prop.GetValue(obj) as IPacketSerializable;
                    string valueJson = value != null ? ToJson(value) : "null";
                    sb.Append(valueJson);
                }
                else
                {
                    sb.Append($"{prop.GetValue(obj)}");
                }

                if (prop != lastProp)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
