using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Threading;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameNetworkingShared.Protocols
{
    public abstract class UDP : IProtocol
    {
        public UdpClient Socket { get; set; }

        private IPEndPoint endPoint;
        protected IPEndPoint EndPoint { get => endPoint; set => endPoint = value; }

        private ITaskManager TaskManager => Threading.TaskManager.Instance;

        protected abstract Dictionary<Type, PacketHandler> PacketHandlers { get; }

        public int Id { get; set; } = -1;

        public event DisconnectHandler OnDisconnect;

        public abstract void Connect(int localPortOrId);

        protected UDP()
        {
            // Empty Ctor
        }

        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                byte[] data = Socket.EndReceive(result, ref endPoint);
                Socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    // Lihora
                    return;
                }

                HandleData(data, Id);
            } 
            catch (Exception ex)
            {
                LogFactory.Instance.Error($"Error while receiving UDP data: {ex}");
            }
        }

        public abstract void SendData(PacketBase packet, IPEndPoint endPoint = null);

        protected void HandleData(byte[] data, int id)
        {
            byte[] realData = null;
            using (Packet packet = new Packet(data))
            {
                int packetLength = packet.ReadInt();
                realData = packet.ReadBytes(packetLength);
            }

            TaskManager.QueueNewTask(() =>
            {
                using (Packet packet = new Packet(realData))
                {
                    HandlePacketData(packet, id);
                }
            });
        }

        private void HandlePacketData(Packet packet, int id)
        {
            int packetMessageId = packet.ReadInt();
            Type packetMessageType = packetMessageId.TypeById();

            if (packetMessageType != null)
            {
                PacketHandlers[packetMessageType].Invoke(packet, id);
                return;
            }

            LogFactory.Instance.Error($"Could not find type for packet message id {packetMessageId}");
        }

        public void Disconnect()
        {
            Socket?.Dispose();
            EndPoint = null;
        }
    }
}
