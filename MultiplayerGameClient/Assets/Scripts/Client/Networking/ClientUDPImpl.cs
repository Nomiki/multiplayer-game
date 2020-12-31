using Assets.Scripts.Common;
using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Assets.Scripts.Client.Networking
{
    public class ClientUDPImpl : UDP
    {
        public ClientUDPImpl() : base()
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(ClientConstants.ServerIp), Constants.ServerPort);
            OnDisconnect += ClientManager.DisconnectClient;
        }

        protected override Dictionary<Type, PacketHandler> PacketHandlers
            => new Dictionary<Type, PacketHandler>()
            {
                { typeof(UdpTest), ClientHandle.HandleUdpTest },
                { typeof(PlayerPosition), ClientHandle.HandlePlayerPosition },
            };

        public override void Connect(int localPort)
        {
            Socket = new UdpClient(localPort);
            Socket.Connect(EndPoint);

            Socket.BeginReceive(ReceiveCallback, null);

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }

            LogFactory.Instance.Debug($"Successfully connected to endpoint {EndPoint}");
        }

        public override void SendData(PacketBase packet, IPEndPoint endPoint = null)
        {
            try
            {
                packet.InsertInt(Id);

                Socket?.BeginSend(packet.ToArray(), packet.Length, null, null);
            }
            catch (Exception ex)
            {
                LogFactory.Instance.Error($"Error while sending UDP data: {ex}");
            }
        }

        public void SendMessage<T>(T data) where T : IPacketSerializable
        {
            using (Packet packet = new Packet())
            {
                packet.WriteObj(data);
                SendMessage(packet);
            }
        }

        private void SendMessage(Packet packet)
        {
            packet.WriteLength();
            SendData(packet);
        }
    }
}
