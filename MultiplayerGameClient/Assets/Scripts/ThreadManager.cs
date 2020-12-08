using GameNetworkingShared.Threading;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    private void Update()
    {
        TaskManager.Instance.RunQueuedTasks();
    }
}