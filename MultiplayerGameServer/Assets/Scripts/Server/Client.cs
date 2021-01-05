using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Protocols;
using System.Net;
using System.Linq;
using GameNetworkingShared.Logging;
using MultiplayerGameServer.Generic;
using GameNetworkingShared.Threading;

namespace MultiplayerGameServer.Server
{
    public class Client : GameNetworkingShared.Generic.Client
    {
        public int Id { get; private set; }
        public override TCP Tcp { get; protected set; }
        public IPEndPoint UdpEndpoint { get; private set; }
        public PlayerPacket PlayerPacket { get; set; }
        public Player Player { get; set; }

        // TODO
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

        public void SpawnIntoGame(string username, int shipModelId)
        {
            PlayerPacket = new PlayerPacket()
            {
                Id = Id,
                ShipModelId = shipModelId,
                Username = username,
                Position = new PlayerPosition() { Id = Id }
            };

            Player = ServerManager.Instance.InstatiatePlayer(PlayerPacket);

            LogFactory.Instance.Debug($"Spawning Player {PlayerPacket.ToJson()} into game...");
            foreach (Client client in Server.Instance.Clients.Values.Where(x => x.PlayerPacket != null && x.Id != Id))
            {
                // Spawn every "other" player for the current spawned player
                ServerSend.SpawnPlayer(Id, client.PlayerPacket);
            }

            foreach (Client client in Server.Instance.Clients.Values.Where(x => x.PlayerPacket != null))
            {
                // Spawn the new player for all existing players in the session
                ServerSend.SpawnPlayer(client.Id, PlayerPacket);
            }
        }

        public override void Disconnect()
        {
            LogFactory.Instance.Debug($"Client {Id}: {Tcp?.Socket?.Client.RemoteEndPoint} has disconnected.");
            TaskManager.Instance.QueueNewTask(() =>
            {
                LogFactory.Instance.Debug($"Destroying {Player} object {Player?.gameObject}");
                UnityEngine.Object.Destroy(Player.gameObject);
                base.Disconnect();
                UdpEndpoint = null;
                PlayerPacket = null;
                Player = null;
            });
        }
    }
}
