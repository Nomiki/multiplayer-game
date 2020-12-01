using GameNetworkingShared.Logging;
using System;

namespace MultiplayerGameServer.Logging
{
    public class ConsoleLogger : ILog
    {
        private static ConsoleLogger instance;

        public static ConsoleLogger Instance = instance ?? (instance = new ConsoleLogger());

        public static void Initiate()
        {
            LogFactory.Instance.SubscribeLogger(Instance);
        }

        private ConsoleLogger()
        {
            // Empty ctor
        }

        public void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
