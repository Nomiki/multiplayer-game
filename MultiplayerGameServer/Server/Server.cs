﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace MultiplayerGameServer.Server
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        private static TcpListener tcpListener;

        public static Dictionary<int, Client> Clients { get; private set; }

        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;

            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        }

        private static void TCPConnectCallback(IAsyncResult result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"incoming connection from {client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (Clients[i].Tcp.Socket == null)
                {
                    Clients[i].Tcp.Connect(client);
                    return;
                }
            }

            Console.WriteLine("reached max clients");
        }

        private static void InitializeServerData()
        {
            Clients = new Dictionary<int, Client>();

            for (int i = 1; i <= MaxPlayers ; i++)
            {
                Clients.Add(i, new Client(i));
            } 
        }
    }
}
