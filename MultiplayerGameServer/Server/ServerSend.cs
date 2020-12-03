using GameNetworkingShared.Packet;

namespace MultiplayerGameServer.Server
{
    public class ServerSend
    {
        public static void Welcome(int toClient, string msg)
        {
            using(PacketBase packet = new PacketBase((int)ServerPackets.welcome))
            {
                packet.Write(msg);
                packet.Write(toClient);
                SendTcpData(toClient, packet);
            }
        }

        private static void SendTcpData(int toClient, PacketBase packet)
        {
            packet.WriteLength();
            Server.Clients[toClient].Tcp.SendData(packet);
        }

        private static void SendTcpDataToAll(PacketBase packet)
        {
            SendTcpDataToAll(-1, packet);
        }

        private static void SendTcpDataToAll(int exceptClient, PacketBase packet)
        {
            packet.WriteLength();
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    Server.Clients[i].Tcp.SendData(packet);
                }
            }
        }
    }
}
