using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Packets
{
    public static class GenericTCPSend
    {
        public static void SendMessage<T>(this TCP tcp, T data) where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                tcp.SendMessage(packet);
            }
        }

        private static void SendMessage(this TCP tcp, Packet packet)
        {
            packet.WriteLength();
            tcp.SendData(packet);
        }

        public static void SendTcpMessageToAll<T>(this TCP[] tcps, T data, int exceptClient = -1) where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                tcps.SendTcpMessageToAll(packet, exceptClient);
            }
        }

        private static void SendTcpMessageToAll(this TCP[] tcps, Packet packet, int exceptClient = -1)
        {
            packet.WriteLength();
            for (int i = 0; i < tcps.Length; i++)
            {
                if (i != exceptClient)
                {
                    tcps[i].SendData(packet);
                }
            }
        }
    }
}
