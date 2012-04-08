using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerCommand : PlayerEvent
    {
        public delegate void OnCall(OnPlayerCommand eventargs);
        protected string _cmd;
        protected string[] args;
        public OnPlayerCommand(Player p, string cmd, string[] args) : base(p) { 
            this._cmd = cmd; this.args = args; 
        }
        internal OnPlayerCommand() { }

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

        public override bool IsCancelable
        {
            get { return true; }
        }
        /// <summary>
        /// Call the event
        /// </summary>
        public override  void Call()
        {
            Muffins.cache.ForEach(e =>
            {
                if (e.type.GetType() == GetType() && ((Player)(e.target) == Player || e.target == null))
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
