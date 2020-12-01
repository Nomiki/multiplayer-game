using Assets.Scripts.Logging;
using Assets.Scripts.Networking;
using GameNetworkingShared.Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClientManager : MonoBehaviour
    {
        public static ClientManager Instance { get; set; }

        private Client Client { get; set; }

        private void Awake()
        {
            UnityLogger.Initiate();
            LogFactory.Instance.Debug("Greetings!");
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                LogFactory.Instance.Debug("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

        private void Start()
        {
            Client = new Client();
            Client.Start();
        }

        public void ConnectToServer()
        {
            Client.Tcp.Connect();
        }
    }
}
