/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 4/28/2012
 * Time: 12:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MCForge.World;
using MCForge.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MCForge.API.World
{
	/// <summary>
	/// Description of OnLevelLoad.
	/// </summary>
	public class OnLevelSave : WorldEvent
	{
		/// <summary>
		/// Creates a new event.  This is NOT meant to be used by user-code, only internally by events.
		/// </summary>
		/// <param name="callback">the method used for the delegate to callback upon event fire</param>
		/// <param name="target">The target Level we want the event for.</param>
		private OnLevelSave(OnCall callback, Level target) : base(target) {
			_queue += callback;
		}

		/// <summary>
		/// This is meant to be called from the code where you mean for the event to happen.
		/// 
		/// In this case, it is called from the command processing code.
		/// </summary>
		/// <param name="p">The player that caused the event.</param>
		/// <param name="oldPos">The old position of the person.</param>
		internal static bool Call(Level l) {
			Logger.Log("Calling OnLevelSave event", LogType.Debug);
			//Event was called from the code.
			List<OnLevelSave> opcList = new List<OnLevelSave>();
			//Do we keep or discard the event?
			_eventQueue.ForEach(opc => {
				if (opc.level == null || opc.level == l) {// We keep it
					Level oldPlayer = opc.level; 
					opc._target = l; // Set player that triggered event.
					opc._queue(opc); // fire callback
					opcList.Add(opc); // add to used list
					opc._target = oldPlayer;
				}
			});
			return opcList.Any(pe => pe.cancel); //Return if any canceled the event.
		}

		/// <summary>
		/// The delegate used for callbacks.  The caller will have this method run when the event fires.
		/// </summary>
		/// <param name="e">The Event that fired</param>
		public delegate void OnCall(OnLevelSave e);

		/// <summary>
		/// The queue of delegates to call for the particular tag (One for each event)
		/// </summary>
		private OnCall _queue;

		/// <summary>
		/// The list of all events currently active of a PlayerEvent type.
		/// </summary>
		protected static List<OnLevelSave> _eventQueue = new List<OnLevelSave>(); // Same across all events of this kind

		/// <summary>
		/// Used to register a method to be executed when the event is fired.
		/// </summary>
		/// <param name="callback">The method to call</param>
		/// <param name="target">The level to watch for. (null for any level)</param>
		/// <returns>A reference to the event</returns>
		public static OnLevelSave Register(OnCall callback, Level target) {
			Logger.Log("OnLevelSave registered to the method " + callback.Method.Name, LogType.Debug);
			//We add it to the list here
			OnLevelSave pe = _eventQueue.Find(match => match.level == null);
			if (pe != null)
				//It already exists, so we just add it to the queue.
				pe._queue += callback;
			else {
				//Doesn't exist yet.  Make a new one.
				pe = new OnLevelSave(callback, target);
				_eventQueue.Add(pe);
			}
			return pe;
		}

		/// <summary>
		/// Unregisters the specific event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public static void Unregister(OnLevelSave pe) {
			pe.Unregister();
		}
		/// <summary>
		/// Unregisters the specific event
		/// </summary>
		/// <param name="pe">The event to unregister</param>
		public override void Unregister() {
			_eventQueue.Remove(this);
		}
	}
}
