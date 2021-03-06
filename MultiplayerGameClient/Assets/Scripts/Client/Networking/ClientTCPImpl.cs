﻿using Assets.Scripts.Common;
using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Assets.Scripts.Client.Networking
{
    public class ClientTCPImpl : TCP
    {
        private static Dictionary<Type, PacketHandler> packetHandlers =
            new Dictionary<Type, PacketHandler>()
            {
                { typeof(WelcomeMessage), ClientHandle.Welcome },
                { typeof(PlayerPacket), ClientHandle.HandlePlayerJoin },
                { typeof(PlayerDisconnectedPacket), ClientHandle.PlayerDisconnected },
            };

        protected override Dictionary<Type, PacketHandler> PacketHandlers =>
            packetHandlers;

        public ClientTCPImpl() : base()
        {
            OnDisconnect += ClientManager.DisconnectClient;
        }

        public override void Connect(TcpClient client = null)
        {
            Socket = new TcpClient()
            {
                ReceiveBufferSize = Constants.DataBufferSize,
                SendBufferSize = Constants.DataBufferSize
            };

            ReceiveBuffer = new byte[Constants.DataBufferSize];
            ReceivedData = new Packet();
            Socket.BeginConnect(ClientConstants.ServerIp, Constants.ServerPort,
                ConnectCallback, Socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            Socket.EndConnect(result);

            if (!Socket.Connected)
            {
                LogFactory.Instance.Debug("Socket not connected");
                return;
            }

            Stream = Socket.GetStream();
            Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);
        }
    }
}
