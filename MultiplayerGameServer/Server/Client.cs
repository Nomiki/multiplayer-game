using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Protocols;
using System.Net;
using System.Linq;
using System.Net.Sockets;

namespace MultiplayerGameServer.Server
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public int Id { get; private set; }
        public override TCP Tcp { get; protected set; }
        public IPEndPoint UdpEndpoint { get; private set; }
        public Player Player { get; set; }

        public Client(int id) : base()
        {
            Id = id;
            Start();
        }
        
        public override void Start()
        {
            Tcp = new ServerTCPImpl(Id);
        }

        public void SetUdpEndpoint(IPEndPoint endpoint)
        {
            UdpEndpoint = endpoint;
            ServerSend.UdpTest(Id);
        }

        public void SpawnIntoGame(string username)
        {
            Player = new Player()
            {
                Id = Id,
                Username = username,
                Position = new PlayerPosition()
            };

            foreach (Client client in Server.Clients.Values.Where(x => x.Player != null && x.Id != Id))
            {
                // Spawn every "enemy" player for the current spawned player
                ServerSend.SpawnPlayer(Id, client.Player);
            }

            foreach (Client client in Server.Clients.Values.Where(x => x.Player != null))
            {
                // Spawn the new player for all existing players in the session
                ServerSend.SpawnPlayer(client.Id, Player);
            }
        }
    }
}
