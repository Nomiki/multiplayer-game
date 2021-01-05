using GameNetworkingShared.Logging;
using GameNetworkingShared.Threading;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class ThreadManager : MonoBehaviour
    {
        private ThreadManager Instance { get; set; }

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

        private void Update()
        {
            TaskManager.Instance.RunQueuedTasks();
        }
    }
}