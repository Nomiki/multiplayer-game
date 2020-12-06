using GameNetworkingShared.Logging;
using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Server;
using System;
using System.Net.Sockets;

namespace MultiplayerGameServer.Protocols
{
    public class ServerTCPImpl : TCP
    {
        public int Id { get; protected set; }

        public ServerTCPImpl(int id) : base()
        {
            Id = id;
        }

        public override void Connect(TcpClient socket = null)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Constants.DataBufferSize;
            Socket.SendBufferSize = Constants.DataBufferSize;

            Stream = Socket.GetStream();
            ReceiveBuffer = new byte[Constants.DataBufferSize];

            Stream.BeginRead(ReceiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);

            LogFactory.Instance.Debug($"Socket connected successfully: {socket.Client.RemoteEndPoint}, ID {Id}");

            ServerSend.Welcome(Id, "Welcome to the server!");

            LogFactory.Instance.Debug($"Sent Welcome packet to client {socket.Client.RemoteEndPoint}, ID {Id}");
        }

        protected override void ReceiveCallback(IAsyncResult result)
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
