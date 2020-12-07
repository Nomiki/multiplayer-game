using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;

namespace Assets.Scripts.Packets
{
    public static class ClientSend
    {
        public static void SendWelcomeReceived()
        {
            WelcomeReceivedMessage message = new WelcomeReceivedMessage()
            {
                Username = UIManager.Instance.UsernameField.text,
                ClientId = ClientManager.Instance.Client.Id,
            };

            ClientManager.Instance.Client.Tcp.SendMessage(message);
        }
    }    
}
