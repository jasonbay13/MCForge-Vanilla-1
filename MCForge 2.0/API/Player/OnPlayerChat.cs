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
    public class OnPlayerChat : Event, Cancelable, PlayerEvent
    {

        Player p;
        public delegate void OnCall(OnPlayerChat eventargs);
        string message;
        bool _canceled = false;
        
        public OnPlayerChat(Player p, string message) { this.p = p; this.message = message; }

        internal OnPlayerChat() { }

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
        /// Get the message the player sent
        /// </summary>
        /// <returns>The message</returns>
        public string GetMessage()
        {
            return message;
        }

        /// <summary>
        /// Change the message the player sent
        /// </summary>
        /// <param name="message">The new message</param>
        public void SetMessage(string message)
        {
            this.message = message;
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
        /// Get the player connected to the event
        /// </summary>
        /// <returns>The player</returns>
        public Player GetPlayer()
        {
            return p;
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority)
        {
            EventHelper temp = new EventHelper(method, priority, new OnPlayerChat());
            EventHelper.Push(temp);
        }
    }
}
