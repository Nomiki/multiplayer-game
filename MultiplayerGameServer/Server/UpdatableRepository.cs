using MultiplayerGameServer.Generic;
using System.Collections.Generic;

namespace MultiplayerGameServer.Server
{
    public class UpdatableRepository : IUpdatable
    {
        private static UpdatableRepository instance;

        public static UpdatableRepository Instance => instance ?? (instance = new UpdatableRepository());

        private List<IUpdatable> Updatables { get; set; } = new List<IUpdatable>();
        
        public void Register(IUpdatable obj)
        {
            Updatables.Add(obj);
        }

        public void Update()
        {
            foreach(IUpdatable updatable in Updatables)
            {
                updatable.Update();
            }
        }
    }
}
