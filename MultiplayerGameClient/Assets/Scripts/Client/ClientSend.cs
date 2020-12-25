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
                ClientId = ClientManager.Instance.Client.Id,
                ShipModelId = UIManager.Instance.Selection,
                Username = UIManager.Instance.UsernameField.text,
            };

            Client.Tcp.SendMessage(message);
        }

        internal static void SendUdpTestReceived()
        {
            UdpTest test = new UdpTest()
            {
                Message = "UDP_CONFIRMED"
            };

            Client.Udp.SendMessage(test);
        }
    }
}
