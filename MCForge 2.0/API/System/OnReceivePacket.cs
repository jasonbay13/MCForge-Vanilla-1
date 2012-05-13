using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.Utils;


namespace MCForge.API.System {

    /// <summary>
    /// Event for recieveing all overflowed packets, this event can be canceled
    /// </summary>
    public class OnReceivePacket : SystemEvent {

        /// <summary>
        /// Data Recieved
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Player that recieved the packet
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Delegate for recieveing packets
        /// </summary>
        /// <param name="args">OnReceivePacket class to recieve</param>
        public delegate void OnPacket(OnReceivePacket args);


        internal OnReceivePacket(Player sender, byte[] data) {
            Player = sender;
            Data = data;
        }

        public bool IsCanceled { get; set; }
        /// <summary>
        /// Calls every event
        /// </summary>
        public void Call() {
        	Logger.Log("Calling OnReceivePacket", LogType.Debug);
            ToCall.ForEach(method => {
                method(this);
            });
        }

        private static readonly List<OnPacket> ToCall = new List<OnPacket>();

        /// <summary>
        /// Register an OnPacket event to the server
        /// </summary>
        /// <param name="Event">OnPacket Delegate to register</param>
        public static void Register(OnPacket Event) {
            ToCall.Add(Event);
        }

        /// <summary>
        /// Unregister the event
        /// </summary>
        /// <param name="Event">Event to unregister</param>
        public static void Unregister(OnPacket Event) {
            if (ToCall.Contains(Event))
                ToCall.Remove(Event);
        }

        [Obsolete("Please use Unregister(Onpacket)", true)]
        public override void Unregister() {
            throw new NotImplementedException();
        }
    }
}