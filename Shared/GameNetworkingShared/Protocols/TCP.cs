using GameNetworkingShared.Logging;
using GameNetworkingShared.Packets;
using System;
using System.Net.Sockets;

namespace GameNetworkingShared.Protocols
{
    public abstract class TCP
    {
        public TcpClient Socket { get; protected set; }
        protected NetworkStream Stream { get; set; }
        protected byte[] ReceiveBuffer { get; set; }

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
                    Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                LogFactory.Instance.Error($"Error while sending data: {ex}");
            }
        }
    }
}
