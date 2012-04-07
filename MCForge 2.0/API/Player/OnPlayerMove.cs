using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Core;

namespace MCForge.API.PlayerEvent
{
    public class OnPlayerMove : PlayerEvent
    {
        Vector3 oldpos;
        Vector3 currentpos;
        public delegate void OnCall(OnPlayerMove args);
        public OnPlayerMove(Player p, Vector3 oldpos, Vector3 currentpos) : base(p) { 
            this.oldpos = oldpos; this.currentpos = currentpos;
        }
        internal OnPlayerMove() { }
        public override bool IsCancelable
        {
            get { return true; }
        }
        public Vector3 GetPos()
        {
            return currentpos;
        }
        public Vector3 GetOldPos()
        {
            return oldpos;
        }
        public override void Call()
        {
            Muffins.muffinbag.ForEach(e =>
            {
                if (e.type.GetType() == GetType() && ((Player)(e.target) == p || e.target == null))
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
            if (IsCanceled)
                GetPlayer().SendToPos(oldpos, GetPlayer().Rot);
        }
        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority, object datapass = null, Player target = null)
        {
            Muffins temp = new Muffins(method, priority, new OnPlayerMove(), datapass, target);
            Muffins.GiveDerpyMuffins(temp);
        }
    }
}
