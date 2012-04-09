using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;


namespace MCForge.API.System {

    /// <summary>
    /// Event for recieveing all messages, unedited and unmodified.
    /// </summary>
    public class OnPlayerChatRaw : Event, Cancelable {

        /// <summary>
        /// Data Recieved
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Player that recieved the message
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Delegate for recieveing messages
        /// </summary>
        /// <param name="args">OnReceivePacket class to recieve</param>
        public delegate void OnMessage(OnPlayerChatRaw args);


        internal OnPlayerChatRaw(Player sender, string message) {
            Player = sender;
            Message = message;
        }

        /// <summary>
        /// Gets or Sets the canceledness of the event
        /// </summary>
        public bool IsCanceled { get; set; }


        /// <summary>
        /// Calls every event
        /// </summary>
        public void Call() {
            ToCall.ForEach(method => {
                method(this);
            });
        }

        private static readonly List<OnMessage> ToCall = new List<OnMessage>();

        /// <summary>
        /// Register an OnPacket event to the server
        /// </summary>
        /// <param name="Event">OnPacket Delegate to register</param>
        public static void Register(OnMessage Event) {
            ToCall.Add(Event);
        }

        /// <summary>
        /// Unregister the event
        /// </summary>
        /// <param name="Event">Event to unregister</param>
        public static void Unregister(OnMessage Event) {
            if (ToCall.Contains(Event))
                ToCall.Remove(Event);
        }
    }
}
