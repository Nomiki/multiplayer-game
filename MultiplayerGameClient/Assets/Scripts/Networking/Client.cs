using GameNetworkingShared.Protocols;

namespace Assets.Scripts.Networking
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public override TCP Tcp { get; protected set; }

        public UDP Udp { get; protected set; }

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
    }
}