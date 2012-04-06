using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerMove : Event, Cancelable, PlayerEvent
    {
        Point3 oldpos;
        Point3 currentpos;
        Player p;
        object datapass;
        public delegate void OnCall(OnPlayerMove args);
        bool _canceled;
        public OnPlayerMove(Player p, Point3 oldpos, Point3 currentpos) { this.oldpos = oldpos; this.currentpos = currentpos; this.p = p; }
        internal OnPlayerMove() { }

        public bool IsCanceled { get { return _canceled; } }

        public void Cancel(bool value)
        {
            _canceled = value;
        }

        public Player GetPlayer()
        {
            return p;
        }
        public Point3 GetPos()
        {
            return currentpos;
        }
        public Point3 GetOldPos()
        {
            return oldpos;
        }
        public override void Call()
        {
            Muffins.cache.ForEach(e =>
            {
                if (e.type.GetType() == GetType() && ((Player)(e.target) == p || e.target == null))
                {
                    datapass = e.datapass;
                    ((OnCall)e.Delegate)(this);
                }
            });
            if (IsCanceled)
                p.SendToPos(oldpos, p.Rot);
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
