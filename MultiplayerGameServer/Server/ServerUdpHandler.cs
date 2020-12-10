using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiplayerGameServer.Server
{
    public class ServerUdpHandler : UDP
    {
        protected override Dictionary<Type, PacketHandler> PacketHandlers => throw new NotImplementedException();

        private static UdpClient udpListener;

        //private static Dictionary<int, Client> Clients => Server.Clients;

        public ServerUdpHandler(int port) : base()
        {
            udpListener = new UdpClient(port);
            udpListener.BeginReceive(ReceiveCallback, null);
        }

        public override void Connect(int id)
        {
        }

        protected override void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                IPEndPoint endPoint = null;
                byte[] data = udpListener.EndReceive(result, ref endPoint);
                udpListener.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                using Packet packet = new Packet(data);
                int clientId = packet.ReadInt();
                if (Server.Clients.ContainsKey(clientId))
                {
                    Client client = Server.Clients[clientId];
                    if (client.UdpEndpoint == null)
                    {
                        Server.Clients[clientId].UdpEndpoint = endPoint;
                        return;
                    } 
                    else if (!client.UdpEndpoint.Equals(endPoint))
                    {
                        LogFactory.Instance.Error($"Client with id {clientId} tries to connect with a different endpoint {endPoint}, registered endpoint: {client.UdpEndpoint}");
                        return;
                    }
                    else
                    {

                    }
                }
                else
                {
                    LogFactory.Instance.Error($"Client with id {clientId} tries to connect with UDP but doesn't exist");
                    return;
                }
            }
        }
    }
}
