using Assets.Scripts.Networking;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Protocols;
using System;

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

        internal static void SendUdpTestReceived()
        {
            UdpTest test = new UdpTest()
            {
                Message = "Halo Ayelet UDP"
            };

            ClientUDPImpl client = ClientManager.Instance.Client.Udp as ClientUDPImpl;
            client?.SendMessage(test);
        }
    }    
}
