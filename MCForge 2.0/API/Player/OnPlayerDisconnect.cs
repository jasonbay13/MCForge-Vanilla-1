using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerDisconnect : PlayerEvent
    {
        public delegate void OnCall(OnPlayerDisconnect args);
        string reason;
        public OnPlayerDisconnect(Player p, string reason) : base(p) { this.reason = reason; }
        internal OnPlayerDisconnect() { }
        public string GetReason()
        {
            return reason;
        }
        public override bool IsCancelable
        {
            get { return false; }
        }
        public override void Call()
        {
            Muffins.muffinbag.ForEach(e =>
            {
                if (e.type.GetType() == GetType() && ((Player)(e.target) == Player || e.target == null))
                {
                    datapass = e.datapass;
                    ((OnCall)e.Delegate)(this);
                    if (_unregister)
                    {
                        _unregister = false;
                        Muffins.muffinbag.Remove(e);
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
            Muffins temp = new Muffins(method, priority, new OnPlayerDisconnect(), datapass, target);
            Muffins.GiveDerpyMuffins(temp);
        }
    }
}
