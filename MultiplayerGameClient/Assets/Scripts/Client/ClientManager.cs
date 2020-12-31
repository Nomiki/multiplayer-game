using Assets.Scripts.Logging;
using GameNetworkingShared.Logging;
using UnityEngine;
using CLIENT = Assets.Scripts.Client.Networking.Client;

namespace Assets.Scripts.Client
{
    public class ClientManager : MonoBehaviour
    {
        public static ClientManager Instance { get; set; }

        public CLIENT Client { get; private set; }

        public bool IsConnected { get; set; }

        private void Awake()
        {
            UnityLogger.Initiate();
            if (Instance == null)
            {
                LogFactory.Instance.Debug($"loaded {this.GetType()}: {this}");
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
            Client = new CLIENT();
            Client.Start();
        }

        public void ConnectToServer()
        {
            Client.Tcp.Connect();
            IsConnected = true;
        }

        private void OnApplicationQuit()
        {
            DisconnectClient();
        }

        public static void DisconnectClient(int id = -1)
        {
            if (Instance.IsConnected)
            {
                Instance.Client?.Disconnect();
            }
        }
    }
}
