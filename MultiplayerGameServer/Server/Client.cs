using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Protocols;
using System.Net.Sockets;

namespace MultiplayerGameServer.Server
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public int Id { get; private set; }
        public override TCP Tcp { get; protected set; }
        
        public Client(int id) : base()
        {
            Id = id;
            Start();
        }
        
        public override void Start()
        {
            Tcp = new ServerTCPImpl(Id);
        }

        public void Connect(TcpClient socket)
        {
            Tcp.Connect();
            ServerSend.Welcome(Id, "Welcome to the server");
        }
    }
}
