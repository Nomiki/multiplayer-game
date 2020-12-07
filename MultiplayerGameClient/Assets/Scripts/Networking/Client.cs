using GameNetworkingShared.Protocols;

namespace Assets.Scripts.Networking
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public override TCP Tcp { get; protected set; }

        public int Id { get; set; }

        public override void Start()
        {
            Tcp = new ClientTCPImpl();
        }
    }
}