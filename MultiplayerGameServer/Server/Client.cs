using GameNetworkingShared.Protocols;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerGameServer.Server
{
    public class Client
    {
        public int Id { get; private set; }
        public TCP Tcp { get; private set; }
        
        public Client(int id)
        {
            Id = id;
            Tcp = new TCP(Id);
        }
    }

}
