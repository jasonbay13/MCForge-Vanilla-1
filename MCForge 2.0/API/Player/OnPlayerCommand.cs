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
        object datapass;
        bool _unregister;
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

        public void Unregister(bool value)
        {
            _unregister = value;
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

        public object GetData()
        {
            return datapass;
        }
        /// <summary>
        /// Call the event
        /// </summary>
        public override void Call()
        {
            Muffins.cache.ForEach(e =>
            {
                if (e.type.GetType() == GetType() && ((Player)(e.target) == p || e.target == null))
                {
                    datapass = e.datapass;
                    ((OnCall)e.Delegate)(this);
                    if (_unregister)
                    {
                        _unregister = false;
                        Muffins.cache.Remove(e);
                    }
                }
            });
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority, object datapass = null, Player target = null)
        {
            Muffins temp = new Muffins(method, priority, new OnPlayerCommand(), datapass, target);
            Muffins.GiveDerpyMuffins(temp);
        }

    }
}
