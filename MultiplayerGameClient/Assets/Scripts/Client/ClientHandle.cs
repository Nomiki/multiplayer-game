using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using System;
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

        internal static void PlayerDisconnected(Packet packet, int fromClient)
        {
            PlayerDisconnectedPacket data = packet.ReadObj<PlayerDisconnectedPacket>();
            LogFactory.Instance.Debug($"Player {data.Id} disconnected received, cleaning up...");

            if (data.Id == ClientManager.Instance.Client.Id)
            {
                LogFactory.Instance.Error($"Detected self disconnect, this should never happen :(");
            }

            GameManager.Instance.DisconnectPlayer(data.Id);
        }

        public static void HandleUdpTest(Packet packet, int fromClient = -1)
        {
            UdpTest test = packet.ReadObj<UdpTest>();
            LogFactory.Instance.Debug($"Got UdpTest message: '{test.Message}'");

            ClientSend.SendUdpTestReceived();
        }

        public static void HandlePlayerJoin(Packet packet, int fromClient = -1)
        {
            PlayerPacket player = packet.ReadObj<PlayerPacket>();
            LogFactory.Instance.Debug($"Got Player: {player.ToJson()}");

            GameManager.Instance.SpawnPlayer(player);
        }

        public static void HandlePlayerPosition(Packet packet, int fromClient = -1)
        {
            PlayerPosition position = packet.ReadObj<PlayerPosition>();
            GameManager.Instance.Players[position.Id].SetPlayerPosition(position);
        }
    }
}
