using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;
using System;
using System.Linq;

namespace MultiplayerGameServer.Server
{
    public class ServerSend
    {
        internal static void Welcome(int clientId, string msg)
        {
            WelcomeMessage welcomeMessage = new WelcomeMessage()
            {
                Message = msg,
                ClientId = clientId,
            };

            Server.Instance.Clients[clientId].Tcp.SendMessage(welcomeMessage);
        }

        internal static void UdpTest(int clientId)
        {
            UdpTest test = new UdpTest()
            {
                Message = "UDP_OK",
            };

            Server.Instance.UdpHandler.SendMessage(clientId, test);
        }

        internal static void SpawnPlayer(int id, PlayerPacket player)
        {
            Server.Instance.Clients[id].Tcp.SendMessage(player);
        }

        internal static void UpdatePlayerPositionRotation(PlayerPosition position)
        {
            Server.Instance.UdpHandler.SendMessageToAll(position);
        }

        internal static void PlayerDisconnected(int id)
        {
            PlayerDisconnectedPacket data = new PlayerDisconnectedPacket() { Id = id };
            Server.Instance.Clients.Values.Select(x => x.Tcp).ToArray().SendMessageToAll(data);
        }
    }
}
