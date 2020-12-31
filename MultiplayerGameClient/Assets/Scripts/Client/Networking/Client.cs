using GameNetworkingShared.Protocols;

namespace Assets.Scripts.Client.Networking
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public override TCP Tcp { get; protected set; }

        public ClientUDPImpl Udp { get; protected set; }

        private int id;
        public int Id { 
            get => id; 
            set
            {
                id = value;
                Udp.Id = id;
            }
        }

        public override void Start()
        {
            Tcp = new ClientTCPImpl();
            Udp = new ClientUDPImpl();
        }

        public override void Disconnect()
        {
            Tcp.Socket.Close();
            base.Disconnect();
            Udp.Socket.Close();
            Udp.Disconnect();
            Udp = null;
        }
    }
}