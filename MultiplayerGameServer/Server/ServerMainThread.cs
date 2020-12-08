using GameNetworkingShared.Logging;
using GameNetworkingShared.Threading;
using MultiplayerGameServer.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerGameServer.Server
{
    public class ServerMainThread
    {
        private static bool isRunning = false;

        public static void Run()
        {
            isRunning = true;
            Task.Run(async () =>
            {
                LogFactory.Instance.Info($"Server main task started with {ServerConsts.TicksPerSecond} ticks per second...");
                DateTime nextIterationTime = DateTime.Now;
                while (isRunning)
                {
                    nextIterationTime = nextIterationTime.AddMilliseconds(ServerConsts.MsPerTick);
                    TaskManager.Instance.RunQueuedTasks();
                    DateTime now = DateTime.Now;
                    if (nextIterationTime > now)
                    {
                        await Task.Delay(nextIterationTime - now);
                    }
                }
            });
        }
    }
}
