using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;

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

            Server.Clients[clientId].Tcp.SendMessage(welcomeMessage);
        }

        public static void UdpTest(int clientId)
        {
            UdpTest test = new UdpTest()
            {
                Message = "You have successfully connected via udp, good job ma man",
            };

            Server.UdpHandler.SendMessage(clientId, test);
        }
    }
}
