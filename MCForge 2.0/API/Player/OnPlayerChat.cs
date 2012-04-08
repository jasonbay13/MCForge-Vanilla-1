using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.PlayerEvent
{
    /// <summary>
    /// The OnPlayerChat event is excuted everytime a player chats on the server
    /// This event can be canceled
    /// </summary>
    public class OnPlayerChat : PlayerEvent
    {

        public delegate void OnCall(OnPlayerChat eventargs);
        public OnPlayerChat(Player p, string message) : base(p) { 
            this.Message = message; 
        }

        internal OnPlayerChat() { }
         /// <summary>
        /// Call the event
        /// </summary>
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
        /// Get or set the message the player sent
        /// </summary>
        /// <returns>The message</returns>
        public string Message { get; set; }

        public override bool IsCancelable
        {
            get { return true; }
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority, object passdata = null, Player target = null)
        {
            Muffins temp = new Muffins(method, priority, new OnPlayerChat(), passdata, target);
            Muffins.GiveDerpyMuffins(temp);
        }
    }
}
