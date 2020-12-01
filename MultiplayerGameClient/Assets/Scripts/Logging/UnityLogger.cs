using UnityEngine;
using GameNetworkingShared.Logging;

namespace Assets.Scripts.Logging
{
    public class UnityLogger : ILog
    {
        private static UnityLogger instance;

        public static UnityLogger Instance = instance ?? (instance = new UnityLogger());

        public static void Initiate()
        {
            LogFactory.Instance.SubscribeLogger(Instance);
        }

        private UnityLogger()
        {
            // Empty ctor
        }

        public void Debug(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        public void Error(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public void Info(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}
