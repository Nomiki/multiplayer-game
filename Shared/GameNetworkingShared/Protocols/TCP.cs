using GameNetworkingShared.Logging;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GameNetworkingShared.Protocols
{
    public class TCP
    {
        public TcpClient Socket { get; private set; }
        public int Id { get; private set; }
        private NetworkStream Stream { get; set; }
        private byte[] ReceiveBuffer { get; set; }

        public TCP(int id)
        {
            Id = id;
        }

        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Constants.DataBufferSize;
            Socket.SendBufferSize = Constants.DataBufferSize;

            Stream = Socket.GetStream();
            ReceiveBuffer = new byte[Constants.DataBufferSize];

            Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);

            LogFactory.Instance.Debug($"Socket connected successfully: {socket.Client.RemoteEndPoint}");
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = Stream.EndRead(result);
                if (byteLength <= 0)
                {
                    //TODO: disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(ReceiveBuffer, data, byteLength);

                Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
