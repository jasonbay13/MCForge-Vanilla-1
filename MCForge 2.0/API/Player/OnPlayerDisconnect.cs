using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerDisconnect : Event, PlayerEvent
    {
        Player p;
        public delegate void OnCall(OnPlayerDisconnect args);
        object datapass;
        string reason;
        bool _unregister;
        bool _canceled;
        public OnPlayerDisconnect(Player p, string reason) { this.reason = reason; this.p = p; }
        internal OnPlayerDisconnect() { }
        public Player GetPlayer()
        {
            return p;
        }
        public void Unregister(bool value)
        {
            _unregister = value;
        }
        public string GetReason()
        {
            return reason;
        }
        public object GetData()
        {
            return datapass;
        }
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
            Muffins temp = new Muffins(method, priority, new OnPlayerDisconnect(), datapass, target);
            Muffins.GiveDerpyMuffins(temp);
        }
    }
}
