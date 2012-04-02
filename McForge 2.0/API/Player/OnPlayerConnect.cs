using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerConnect: Event, Cancelable, PlayerEvent
    {
        protected bool _canceled = false;
        public delegate void OnCall(OnPlayerConnect eventargs);
        protected Player p;

        /// <summary>
        /// Create a new Event to call
        /// </summary>
        /// <param name="p">The player connected to the event</param>
        public OnPlayerConnect(Player p) { this.p = p; }

        internal OnPlayerConnect() { }

        public void Cancel(bool value)
        {
            _canceled = value;
        }

        public Player GetPlayer()
        {
            return p;
        }

        public bool IsCanceled { get { return _canceled; } }

        public override void Call()
        {
            EventHelper.cache.ForEach(e =>
                {
                    if (e.GetType() == GetType())
                        ((OnCall)e.Delegate)(this);
                });
            if (IsCanceled && p.isOnline)
                p.Kick("");
        }
        public static void Register(OnCall method, Priority priority)
        {
            EventHelper temp = new EventHelper(method, priority, new OnPlayerConnect());
            EventHelper.Push(temp);
        }
    }
}
