using GameNetworkingShared.Generic;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Threading;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace GameNetworkingShared.Protocols
{
    public abstract class TCP
    {
        public TcpClient Socket { get; protected set; }
        protected NetworkStream Stream { get; set; }
        protected byte[] ReceiveBuffer { get; set; }
        protected abstract Dictionary<Type, PacketHandler> PacketHandlers { get; }
        protected Packet ReceivedData { get; set; }
        public int Id { get; protected set; } = -1;

        protected TCP()
        {
            // Empty ctor
        }

        public abstract void Connect(TcpClient client = null);

        protected abstract void ReceiveCallback(IAsyncResult result);

        public void SendData(PacketBase packet)
        {
            try
            {
                if (Socket != null)
                {
                    Stream.BeginWrite(packet.ToArray(), 0, packet.Length, null, null);
                }
            }
            catch (Exception ex)
            {
                LogFactory.Instance.Error($"Error while sending data: {ex}");
            }
        }

        protected bool HandleData(byte[] data)
        {
            int packetLength = 0;

            ReceivedData.SetBytes(data);

            if (ReceivedData.UnreadLength >= 4)
            {
                packetLength = ReceivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= ReceivedData.UnreadLength)
            {
                byte[] packetBytes = ReceivedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        HandlePacketId(packetId, packet);
                    }
                });

                packetLength = 0;

                if (ReceivedData.UnreadLength >= 4)
                {
                    packetLength = ReceivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            return packetLength <= 1;
        }

        private void HandlePacketId(int packetMessageId, Packet packet)
        {
            Type packetMessageType = packetMessageId.TypeById();

            if (packetMessageType != null)
            {
                PacketHandlers[packetMessageType].Invoke(packet, Id);
                return;
            }

            LogFactory.Instance.Error($"Could not find type for packet message id {packetMessageId}");
        }
    }
}
