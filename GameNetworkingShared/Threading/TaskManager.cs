using GameNetworkingShared.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Threading
{
    public class TaskManager : ITaskManager
    {
        private static TaskManager instance;

        public static TaskManager Instance => instance ?? (instance = new TaskManager());

        private readonly List<Action> taskQueue = new List<Action>();
        private readonly List<Action> taskQueueCopy = new List<Action>();
        private bool newTaskQueued = false;

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="action">The action to be executed on the main thread.</param>
        public void QueueNewTask(Action action)
        {
            if (action == null)
            {
                LogFactory.Instance.Debug("No action to queue.");
                return;
            }

            lock (taskQueue)
            {
                taskQueue.Add(action);
                newTaskQueued = true;
            }
        }

        /// <summary>
        /// Executes all code meant to run on the main thread. 
        /// NOTE: Call this ONLY from the main thread.
        /// </summary>
        public void RunQueuedTasks()
        {
            if (newTaskQueued)
            {
                taskQueueCopy.Clear();
                lock (taskQueue)
                {
                    taskQueueCopy.AddRange(taskQueue);
                    taskQueue.Clear();
                    newTaskQueued = false;
                }

                foreach (Action task in taskQueueCopy)
                {
                    task.Invoke();
                }
            }
        }
    }
}
