using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
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
        protected override Dictionary<Type, PacketHandler> PacketHandlers
            => new Dictionary<Type, PacketHandler>()
            {
                { typeof(UdpTest), ServerHandle.UdpTestReceived }
            };


        public ServerUdpHandler(int port) : base()
        {
            Socket = new UdpClient(port);
            Socket.BeginReceive(ReceiveCallback, null);
        }

        public override void Connect(int id)
        {
        }

        protected override void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                IPEndPoint endPoint = null;
                byte[] data = Socket.EndReceive(result, ref endPoint);
                Socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                using Packet packet = new Packet(data);
                int clientId = packet.ReadInt();
                if (!Server.Clients.ContainsKey(clientId))
                {
                    LogFactory.Instance.Error($"Client with id {clientId} tries to connect with UDP but doesn't exist");
                    return;
                }

                Client client = Server.Clients[clientId];
                if (client.UdpEndpoint == null)
                {
                    Server.Clients[clientId].SetUdpEndpoint(endPoint);
                }
                else if (!client.UdpEndpoint.Equals(endPoint))
                {
                    LogFactory.Instance.Error($"Client with id {clientId} tries to connect with a different endpoint {endPoint}, registered endpoint: {client.UdpEndpoint}");
                }
                else
                {
                    byte[] packetData = packet.ReadBytes(packet.UnreadLength);
                    HandleData(packetData, clientId);
                }
            }
            catch (Exception ex)
            {
                LogFactory.Instance.Error($"Error receiving UDP data {ex}");
            }
        }

        public void SendMessage<T>(int clientId, T data) where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                SendMessage(clientId, packet);
            }
        }

        private void SendMessage(int clientId, Packet packet)
        {
            packet.WriteLength();
            SendData(packet, Server.Clients[clientId].UdpEndpoint);
        }

        public void SendMessageToAll<T>(T data, int exceptClient = -1)
            where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                SendMessageToAll(packet, exceptClient);
            }
        }

        private void SendMessageToAll(Packet packet, int exceptClient = -1)
        {
            packet.WriteLength();
            foreach (Client client in Server.Clients.Values)
            {
                if (client.UdpEndpoint != null && client.Id != exceptClient)
                {
                    SendData(packet, client.UdpEndpoint);
                }
            }
        }

        public override void SendData(PacketBase packet, IPEndPoint endPoint = null)
        {
            try
            {
                if (endPoint == null)
                {
                    throw new ArgumentNullException("endPoint");
                }

                Socket?.BeginSend(packet.ToArray(), packet.Length, endPoint, null, null);
            }
            catch (Exception ex)
            {
                LogFactory.Instance.Error($"Error while sending UDP data: {ex} to {endPoint}");
            }
        }
    }
}
