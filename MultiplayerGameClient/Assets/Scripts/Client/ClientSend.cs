using Assets.Scripts.Client.Networking;
using Assets.Scripts.UI;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;

namespace Assets.Scripts.Client
{
    public static class ClientSend
    {
        private static Networking.Client Client => ClientManager.Instance.Client;

        public static void SendWelcomeReceived()
        {
            WelcomeReceivedMessage message = new WelcomeReceivedMessage()
            {
                Username = UIManager.Instance.UsernameField.text,
                ClientId = ClientManager.Instance.Client.Id,
            };

            Client.Tcp.SendMessage(message);
        }

        internal static void SendUdpTestReceived()
        {
            UdpTest test = new UdpTest()
            {
                Message = "Halo Ayelet UDP"
            };

            Client.Udp.SendMessage(test);
        }
    }
}
