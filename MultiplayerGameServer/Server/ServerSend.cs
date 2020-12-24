using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;
using System;

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

            Server.Clients[clientId].Tcp.SendMessage(welcomeMessage);
        }

        internal static void UdpTest(int clientId)
        {
            UdpTest test = new UdpTest()
            {
                Message = "UDP_OK",
            };

            Server.UdpHandler.SendMessage(clientId, test);
        }

        internal static void SpawnPlayer(int id, Player player)
        {
            Server.Clients[id].Tcp.SendMessage(player);
        }
    }
}
