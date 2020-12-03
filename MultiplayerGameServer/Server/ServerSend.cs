using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;

namespace MultiplayerGameServer.Server
{
    public class ServerSend
    {
        public static void Welcome(int clientId, string msg)
        {
            WelcomeMessage welcomeMessage = new WelcomeMessage()
            {
                Message = msg,
                ClientId = clientId,
            };

            SendTcpMessage(clientId, welcomeMessage);
        }

        private static void SendTcpMessage<T>(int clientId, T data) where T : IPacketSerializable
        {
            using Packet packet = new Packet();
            packet.WriteObj(data);
            SendTcpMessage(clientId, packet);
        }

        private static void SendTcpMessage(int clientId, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[clientId].Tcp.SendData(packet);
        }

        private static void SendTcpMessageToAll<T>(T data, int exceptClient = -1) where T : IPacketSerializable
        {
            using Packet packet = new Packet();
            packet.WriteObj(data);
            SendTcpMessageToAll(packet, exceptClient);
        }

        private static void SendTcpMessageToAll(Packet packet, int exceptClient = -1)
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
