/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 5/13/2012
 * Time: 12:16 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace MCForge.API.Events
{
    public class DataSavedEvent : Event<Player, DataSavedEventArgs> {
    }
    /// <summary>
    /// PlayerBlockChangeEventArgs
    /// </summary>
    public class DataSavedEventArgs : EventArgs {
    	public long UID { get; private set; }
    	
    	public DataSavedEventArgs(long UID) { this.UID = UID; }
    	
        private bool canceled = false;
    }
}
