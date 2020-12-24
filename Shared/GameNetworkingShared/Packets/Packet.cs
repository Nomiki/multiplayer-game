using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameNetworkingShared.Packets
{
    public class Packet : PacketBase
    {
        private static Dictionary<Type, PropertyInfo[]> PropertiesForType { get; set; }
            = new Dictionary<Type, PropertyInfo[]>();

        private static Dictionary<Type, int> TypeIdAttribute { get; set; }
            = new Dictionary<Type, int>();

        private static Dictionary<Type, Action<Packet, object>> WriteFuncs { get; set; }
        private static Dictionary<Type, Func<Packet, object>> ReadFuncs { get; set; }

        static Packet()
        {
            WriteFuncs = new Dictionary<Type, Action<Packet, object>>()
            {
                { typeof(bool),    (p, o) => p.Write((bool)o)   },
                { typeof(byte),    (p, o) => p.Write((byte)o)   },
                { typeof(short),   (p, o) => p.Write((short)o)  },
                { typeof(int),     (p, o) => p.Write((int)o)    },
                { typeof(long),    (p, o) => p.Write((long)o)   },
                { typeof(float),   (p, o) => p.Write((float)o)  },
                { typeof(string),  (p, o) => p.Write((string)o) },
            };

            ReadFuncs = new Dictionary<Type, Func<Packet, object>>()
            {
                { typeof(bool),    (p) => p.ReadBool()   },
                { typeof(byte),    (p) => p.ReadByte()   },
                { typeof(short),   (p) => p.ReadShort()  },
                { typeof(int),     (p) => p.ReadInt()    },
                { typeof(long),    (p) => p.ReadLong()   },
                { typeof(float),   (p) => p.ReadFloat()  },
                { typeof(string),  (p) => p.ReadString() },
            };

            AddPacketSerializableObjectReadWriteMethods();
        }

        private static void AddPacketSerializableObjectReadWriteMethods()
        {
            Type @interface = typeof(IPacketSerializable);
            Assembly assembly = @interface.Assembly;
            IEnumerable<Type> types = assembly.GetTypes()
                .Where(t => @interface.IsAssignableFrom(t) && t != @interface);

            foreach (Type t in types)
            {
                WriteFuncs.Add(t, (p, o) => p.WriteObj((IPacketSerializable)o));
                ReadFuncs.Add(t, (p) => p.ReadNestedObj<IPacketSerializable>(t));
            }
        }

        public Packet() : base() { }

        public Packet(byte[] data) : base(data) { }

        public void WriteObj<T>(T obj) where T : IPacketSerializable
        {
            Type objType = obj.GetType();
            PropertyInfo[] properties = GetOrderedProperties(objType);
            Write(TypeIdAttribute[objType]);
            foreach (PropertyInfo pi in properties)
            {
                object value = pi.GetValue(obj);
                WriteFuncs[pi.PropertyType].Invoke(this, value);
            }
        }

        public T ReadObj<T>() where T : IPacketSerializable
        {
            PropertyInfo[] properties = GetOrderedProperties(typeof(T));
            object obj = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            foreach (PropertyInfo pi in properties)
            {
                object value = ReadFuncs[pi.PropertyType].Invoke(this);
                pi.SetValue(obj, value);
            }

            return (T)obj;
        }

        private T ReadNestedObj<T>(Type objType) where T : IPacketSerializable
        {
            PropertyInfo[] properties = GetOrderedProperties(objType);
            int typeId = ReadInt();
            if (typeId != TypeIdAttribute[objType])
            {
                throw new ArgumentException($"Expected type id {TypeIdAttribute[objType]} got {typeId}");
            }

            object obj = objType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
            foreach (PropertyInfo pi in properties)
            {
                object value = ReadFuncs[pi.PropertyType].Invoke(this);
                pi.SetValue(obj, value);
            }

            return (T)obj;
        }

        private static PropertyInfo[] GetOrderedProperties<T>()
        {
            Type typeofT = typeof(T);
            return GetOrderedProperties(typeofT);
        }

        private static PropertyInfo[] GetOrderedProperties(Type typeofT)
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
            return WriteFuncs.Keys.Contains(t);
        }
    }
}
