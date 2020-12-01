using GameNetworkingShared.Protocols;

namespace Assets.Scripts.Networking
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public override TCP Tcp { get; set; }

        public override void Start()
        {
            Tcp = new ClientTCPImpl();
        }
    }
}