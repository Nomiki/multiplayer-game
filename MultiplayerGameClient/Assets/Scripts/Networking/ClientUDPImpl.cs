using Assets.Scripts.Common;
using GameNetworkingShared.Generic;
using GameNetworkingShared.Packets;
using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Networking
{
    public class ClientUDPImpl : UDP
    {
        public ClientUDPImpl() : base()
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(ClientConstants.ServerIp), Constants.ServerPort);
        }

        protected override Dictionary<Type, PacketHandler> PacketHandlers 
            => throw new NotImplementedException();

        public override void Connect(int localPort)
        {
            Socket = new UdpClient(localPort);
            Socket.Connect(EndPoint);

            Socket.BeginReceive(ReceiveCallback, null);

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }
    }
}
