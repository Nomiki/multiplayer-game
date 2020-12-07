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

            Server.Clients[clientId].Tcp.SendMessage(welcomeMessage);
        }
    }
}
