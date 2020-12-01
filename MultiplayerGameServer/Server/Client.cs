using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Protocols;

namespace MultiplayerGameServer.Server
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public int Id { get; private set; }
        public override TCP Tcp { get; set; }
        
        public Client(int id)
        {
            Id = id;
            Tcp = new ServerTCPImpl(Id);
        }
    }
}
