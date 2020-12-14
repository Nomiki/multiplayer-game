using GameNetworkingShared.Packets;

namespace GameNetworkingShared.Protocols
{
    public static class TcpSender
    {
        public static void SendMessage<T>(this TCP protocolHandler, T data) where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                protocolHandler.SendMessage(packet);
            }
        }

        private static void SendMessage(this TCP protocolHandler, Packet packet)
        {
            packet.WriteLength();
            protocolHandler.SendData(packet);
        }

        public static void SendMessageToAll<T>(this TCP[] protocolHandlers, T data, int exceptClient = -1)
            where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                protocolHandlers.SendMessageToAll(packet, exceptClient);
            }
        }

        private static void SendMessageToAll(this TCP[] protocolHandlers, Packet packet, int exceptClient = -1)
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
