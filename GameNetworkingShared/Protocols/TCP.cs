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
    public abstract class TCP : IProtocol
    {
        public TcpClient Socket { get; protected set; }
        protected NetworkStream Stream { get; set; }
        protected byte[] ReceiveBuffer { get; set; }
        protected abstract Dictionary<Type, PacketHandler> PacketHandlers { get; }
        protected Packet ReceivedData { get; set; }
        protected ITaskManager TaskManager => Threading.TaskManager.Instance;
        public int Id { get; protected set; } = -1;

        public event DisconnectHandler OnDisconnect;

        protected TCP()
        {
            // Empty ctor
        }

        public abstract void Connect(TcpClient client = null);

        protected virtual void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = Stream.EndRead(result);
                if (byteLength <= 0)
                {
                    OnDisconnect.Invoke(Id);
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(ReceiveBuffer, data, byteLength);

                bool handledData = HandleData(data);
                ReceivedData.Reset(handledData);

                Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                OnDisconnect.Invoke(Id);
            }
        }

        public void SendData(PacketBase packet, IPEndPoint endPoint = null)
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
                LogFactory.Instance.Error($"Error while sending TCP data: {ex}");
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
                TaskManager.QueueNewTask(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        HandlePacketData(packet);
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

        private void HandlePacketData(Packet packet)
        {
            int packetMessageId = packet.ReadInt();
            Type packetMessageType = packetMessageId.TypeById();

            if (packetMessageType != null)
            {
                PacketHandlers[packetMessageType].Invoke(packet, Id);
                return;
            }

            LogFactory.Instance.Error($"Could not find type for packet message id {packetMessageId}");
        }

        public virtual void Disconnect()
        {
            Socket?.Close();
            Stream?.Dispose();
            ReceivedData?.Dispose();
            Socket = null;
            Stream = null;
            ReceivedData = null;
            ReceiveBuffer = null;
        }
    }
}
