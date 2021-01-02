using System;
using System.Collections.Generic;

namespace GameNetworkingShared.Logging
{
    public class LogFactory : ILog
    {
        private static LogFactory instance;

        private List<ILog> Loggers { get; set; }

        public static LogFactory Instance => instance ?? (instance = new LogFactory());

        private LogFactory()
        {
            // empty ctor
        }

        public void SubscribeLogger(ILog logger)
        {
            if (Loggers == null)
            {
                Loggers = new List<ILog>();
            }

            if (!Loggers.Contains(logger))
            {
                Loggers.Add(logger);
            }
        }

        public void UnsubscribeLogger(ILog logger)
        {
            if (Loggers == null || !Loggers.Contains(logger))
            {
                return;
            }

            Loggers.Remove(logger);
        }

        public void Debug(string message)
        {
            string sentMessage = AddTimeAndTypeToMessage(message, LogType.Debug);
            foreach (ILog logger in Loggers)
            {
                logger.Debug(sentMessage);
            }
        }

        public void Info(string message)
        {
            string sentMessage = AddTimeAndTypeToMessage(message, LogType.Info);
            foreach (ILog logger in Loggers)
            {
                logger.Info(sentMessage);
            }
        }

        public void Error(string message)
        {
            string sentMessage = AddTimeAndTypeToMessage(message, LogType.Error);
            foreach (ILog logger in Loggers)
            {
                logger.Error(sentMessage);
            }
        }

        private string AddTimeAndTypeToMessage(string message, LogType logType)
        {
            return $"{DateTime.Now:s} - {logType.ToString().ToUpper()} - {message}";
        }
    }
}
