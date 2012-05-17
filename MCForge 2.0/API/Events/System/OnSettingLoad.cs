/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 4/24/2012
 * Time: 7:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MCForge.Utils.Settings;
using MCForge.Core;

namespace MCForge.API.Events
{
	/// <summary>
	/// Description of OnSettingLoad.
	/// </summary>
	/*public class OnSettingLoad : Event<Server, OnSettingLoadArgs>
	{
	}*/
	public class OnSettingLoadArgs : EventArgs, ICancelable
	{
		/// <summary>
		/// The setting that is being loaded
		/// </summary>
		public SettingNode setting { get; private set; }
		
		private bool canceled = false;
        /// <summary>
        /// Whether or not the handling should be canceled
        /// </summary>
        public bool Canceled {
            get { return canceled; }
        }
        /// <summary>
        /// Cancels the handling
        /// </summary>
        public void Cancel() {
            canceled = true;
        }
        /// <summary>
        /// Allows the handling
        /// </summary>
        public void Allow() {
            canceled = false;
        }
        
		public OnSettingLoadArgs(SettingNode setting) {
            this.setting = setting;
        }
	}
}
