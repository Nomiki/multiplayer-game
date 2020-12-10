using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Packets
{
    public static class GenericSend
    {
        public static void SendMessage<T>(this IProtocol protocolHandler, T data) where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                protocolHandler.SendMessage(packet);
            }
        }

        private static void SendMessage(this IProtocol protocolHandler, Packet packet)
        {
            packet.WriteLength();
            protocolHandler.SendData(packet);
        }

        public static void SendTcpMessageToAll<T>(this IProtocol[] protocolHandlers, T data, int exceptClient = -1) 
            where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                protocolHandlers.SendTcpMessageToAll(packet, exceptClient);
            }
        }

        private static void SendTcpMessageToAll(this IProtocol[] protocolHandlers, Packet packet, int exceptClient = -1)
        {
            packet.WriteLength();
            for (int i = 0; i < protocolHandlers.Length; i++)
            {
                if (i != exceptClient)
                {
                    protocolHandlers[i].SendData(packet);
                }
            }
        }
    }
}
