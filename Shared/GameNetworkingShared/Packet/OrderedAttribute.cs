using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GameNetworkingShared.Packet
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class OrderedAttribute : Attribute
    {
        public int Order { get; private set; }

        public OrderedAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }
    }
}
