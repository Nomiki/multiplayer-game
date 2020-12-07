using System;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Protocols;
using MultiplayerGameServer.Logging;
using MultiplayerGameServer.Server;

namespace MultiplayerGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            StartLogger();

            Console.Title= "Game Server";
            Server.Server.Start(5, Constants.ServerPort);
            LogFactory.Instance.Debug($"Listening on {Constants.ServerPort}");
            ServerMainThread.Run();
            Console.ReadKey();
        }

        private static void StartLogger()
        {
            ConsoleLogger.Initiate();
        }
    }
}
