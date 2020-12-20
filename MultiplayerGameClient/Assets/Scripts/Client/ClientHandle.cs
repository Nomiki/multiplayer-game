using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using System.Net;
using UnityEngine;
using CLIENT = Assets.Scripts.Client.Networking.Client;

namespace Assets.Scripts.Client
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet packet, int fromClient = -1)
        {
            WelcomeMessage msg = packet.ReadObj<WelcomeMessage>();

            LogFactory.Instance.Debug($"Got WelcomeMessage: {msg}");

            CLIENT client = ClientManager.Instance.Client;
            client.Id = msg.ClientId;

            ClientSend.SendWelcomeReceived();

            client.Udp.Connect(((IPEndPoint)client.Tcp.Socket.Client.LocalEndPoint).Port);
        }

        public static void HandleUdpTest(Packet packet, int fromClient = -1)
        {
            UdpTest test = packet.ReadObj<UdpTest>();
            LogFactory.Instance.Debug($"Got UdpTest message: '{test.Message}'");

            ClientSend.SendUdpTestReceived();
        }
    }
}
