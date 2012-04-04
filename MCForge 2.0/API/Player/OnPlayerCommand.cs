using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerCommand : Event, Cancelable, PlayerEvent
    {
        protected Player p;
        public delegate void OnCall(OnPlayerCommand eventargs);
        protected bool _canceled;
        protected string _cmd;
        protected string[] args;
        public OnPlayerCommand(Player p, string cmd, string[] args) { this.p = p; this._cmd = cmd; this.args = args; }
        internal OnPlayerCommand() { }

        public Player GetPlayer()
        {
            return p;
        }
        public string GetCmd()
        {
            return _cmd;
        }
        public string[] GetArgs()
        {
            return args;
        }
        public string GetArg(int index)
        {
            return args[index];
        }
        /// <summary>
        /// Is the event canceled
        /// </summary>
        public bool IsCanceled { get { return _canceled; } }

        /// <summary>
        /// Cancel the event
        /// </summary>
        /// <param name="value">True will cancel the event, false will un-cancel the event</param>
        public void Cancel(bool value)
        {
            _canceled = value;
        }

        /// <summary>
        /// Call the event
        /// </summary>
        public override void Call()
        {
            EventHelper.cache.ForEach(e =>
            {
                if (e.GetType() == GetType())
                    ((OnCall)e.Delegate)(this);
            });
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority)
        {
            EventHelper temp = new EventHelper(method, priority, new OnPlayerCommand());
            EventHelper.Push(temp);
        }

    }
}
