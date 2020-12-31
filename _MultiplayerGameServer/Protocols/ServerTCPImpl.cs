using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Server;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MultiplayerGameServer.Protocols
{
    public class ServerTCPImpl : TCP
    {
        private static Dictionary<Type, PacketHandler> packetHandlers = new Dictionary<Type, PacketHandler>()
        {
            { typeof(WelcomeReceivedMessage), ServerHandle.WelcomeReceived }
        };

        protected override Dictionary<Type, PacketHandler> PacketHandlers =>
            packetHandlers;

        public ServerTCPImpl(int id) : base()
        {
            Id = id;
            OnDisconnect += Server.Server.DisconnectClient;
        }

        public override void Connect(TcpClient socket = null)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Constants.DataBufferSize;
            Socket.SendBufferSize = Constants.DataBufferSize;

            Stream = Socket.GetStream();
            ReceiveBuffer = new byte[Constants.DataBufferSize];

            ReceivedData = new Packet();

            Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);

            LogFactory.Instance.Debug($"Socket connected successfully: {socket.Client.RemoteEndPoint}, ID {Id}");

            ServerSend.Welcome(Id, "Welcome to the server!");

            LogFactory.Instance.Debug($"Sent Welcome packet to client {socket.Client.RemoteEndPoint}, ID {Id}");
        }
    }
}
