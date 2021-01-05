using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerGameServer.Server
{
    public class ServerHandle
    {
        public static void WelcomeReceived(Packet packet, int fromClient = -1)
        {
            WelcomeReceivedMessage message = packet.ReadObj<WelcomeReceivedMessage>();

            LogFactory.Instance.Info($"Player {Server.Clients[fromClient].TcpEndpoint} connected successfully and is player {message.ClientId}: {message.Username}");
            if (message.ClientId != fromClient)
            {
                LogFactory.Instance.Error($"Player '{message.Username}', ID: {fromClient} assumed wrong client ID {message.ClientId}");
                return;
            }

            Server.Clients[fromClient].SpawnIntoGame(message.Username, message.ShipModelId);
        }

        public static void PlayerMovementReceived(Packet packet, int fromClient = -1)
        {
            PlayerMovement movement = packet.ReadObj<PlayerMovement>();
            Server.Clients[fromClient].Player?.SetMovement(movement);
        }

        public static void UdpTestReceived(Packet packet, int fromClient = -1)
        {
            UdpTest test = packet.ReadObj<UdpTest>();
            LogFactory.Instance.Info($"Player {fromClient} : {Server.Clients[fromClient].TcpEndpoint} connected successfully in udp: {test.Message}");
        }
    }
}
