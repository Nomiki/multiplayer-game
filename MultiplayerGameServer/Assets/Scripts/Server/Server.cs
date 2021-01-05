using GameNetworkingShared.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace MultiplayerGameServer.Server
{
    public class Server : IDisposable
    {
        private static Server instance;

        public static Server Instance => instance ?? (instance = new Server());

        public int MaxPlayers { get; private set; }
        public int Port { get; private set; }

        private TcpListener tcpListener;

        public ServerUdpHandler UdpHandler { get; set; }

        public Dictionary<int, Client> Clients { get; private set; }

        public void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;

            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            UdpHandler = new ServerUdpHandler(Port);
        }

        private void TCPConnectCallback(IAsyncResult result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            LogFactory.Instance.Debug($"incoming connection from {client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (Clients[i].Tcp.Socket == null)
                {
                    Clients[i].Tcp.Connect(client);
                    return;
                }
            }

            LogFactory.Instance.Debug($"Could not connect {client.Client.RemoteEndPoint}. Reached max clients {MaxPlayers}");
        }

        private void InitializeServerData()
        {
            Clients = new Dictionary<int, Client>();

            for (int i = 1; i <= MaxPlayers ; i++)
            {
                Clients.Add(i, new Client(i));
            } 
        }

        public void DisconnectClient(int id)
        {
            Clients[id]?.Disconnect();
            LogFactory.Instance.Debug($"Done disconnecting client {Clients[id].Tcp.Socket}");
            ServerSend.PlayerDisconnected(id);
        }

        public void Dispose()
        {
            tcpListener.Stop();
            UdpHandler.Disconnect();
        }
    }
}
