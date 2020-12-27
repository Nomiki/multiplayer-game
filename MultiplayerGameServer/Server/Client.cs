using GameNetworkingShared.Objects;
using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Protocols;
using System.Net;
using System.Linq;
using GameNetworkingShared.Logging;
using System.Numerics;
using MultiplayerGameServer.Generic;

namespace MultiplayerGameServer.Server
{
    public class Client : GameNetworkingShared.Generic.Client, IUpdatable
    {
        private const float MoveSpeed = 20f / ServerConsts.TicksPerSecond;
        public int Id { get; private set; }
        public override TCP Tcp { get; protected set; }
        public IPEndPoint UdpEndpoint { get; private set; }
        public Player Player { get; set; }

        public Vector3 Position
        {
            get => new Vector3(Player.Position.X, Player.Position.Y, Player.Position.Z);
            set
            {
                Player.Position.X = value.X;
                Player.Position.Y = value.Y;
                Player.Position.Z = value.Z;
            }
        }

        private PlayerMovement Movement { get; set; } = new PlayerMovement();

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
            Player = new Player()
            {
                Id = Id,
                ShipModelId = shipModelId,
                Username = username,
                Position = new PlayerPosition() { Id = Id }
            };

            LogFactory.Instance.Debug($"Spawning Player {Player.ToJson()} into game...");
            foreach (Client client in Server.Clients.Values.Where(x => x.Player != null && x.Id != Id))
            {
                // Spawn every "other" player for the current spawned player
                ServerSend.SpawnPlayer(Id, client.Player);
            }

            foreach (Client client in Server.Clients.Values.Where(x => x.Player != null))
            {
                // Spawn the new player for all existing players in the session
                ServerSend.SpawnPlayer(client.Id, Player);
            }

            UpdatableRepository.Instance.Register(this);
        }

        public void UpdateMovement()
        {
            float xDirection = (Movement.Right ? 1 : 0) + (Movement.Left ? -1 : 0);
            float yDirection = (Movement.Up ? 1 : 0) + (Movement.Down ? -1 : 0);

            if (xDirection != 0 || yDirection != 0)
            {
                Vector3 up = new Vector3(0, 1, 0);
                Vector3 right = new Vector3(1, 0, 0);
                Vector3 movement = Vector3.Normalize(up * yDirection + right * xDirection) * MoveSpeed;
                Position += movement;
            }
            
            Player.Position.Angle = Movement.Angle;
            ServerSend.UpdatePlayerPositionRotation(Player);
        }

        public void SetMovement(PlayerMovement movement)
        {
            Movement = movement;
        }

        public void Update()
        {
            UpdateMovement();
        }
    }
}
