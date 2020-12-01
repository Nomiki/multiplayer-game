using Assets.Scripts.Common;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Networking
{
    public class ClientTCPImpl : TCP
    {
        public ClientTCPImpl() : base()
        {
            // empty ctor
        }

        public override void Connect(TcpClient client = null)
        {
            Socket = new TcpClient()
            {
                ReceiveBufferSize = Constants.DataBufferSize,
                SendBufferSize = Constants.DataBufferSize
            };

            ReceiveBuffer = new byte[Constants.DataBufferSize];
            Socket.BeginConnect(ClientConstants.ServerIp, Constants.ServerPort, ConnectCallback, Socket);
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

        protected override void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = Stream.EndRead(result);
                if (byteLength <= 0)
                {
                    // TODO: Disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(ReceiveBuffer, data, byteLength);

                // TODO: Handle Data

                Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);
            } 
            catch(Exception ex)
            {
                LogFactory.Instance.Error($"Error handling receive callback. {ex.Message}");
                // TODO: Disconnect
            }
        }
    }
}
