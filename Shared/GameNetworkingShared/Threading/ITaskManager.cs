using System;

namespace GameNetworkingShared.Threading
{
    public interface ITaskManager
    {
        void QueueNewTask(Action action);
        void RunQueuedTasks();
    }
}