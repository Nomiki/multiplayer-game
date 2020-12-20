using GameNetworkingShared.Threading;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class ThreadManager : MonoBehaviour
    {
        private void Update()
        {
            TaskManager.Instance.RunQueuedTasks();
        }
    }
}