using System;
using GameNetworkingShared.Protocols;

namespace MultiplayerGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title= "Nissim Games Inc.";
            Server.Server.Start(5, Constants.Port);
            Console.WriteLine($"Me Listening on {Constants.Port}");
            Console.ReadKey();
        }
    }
}
