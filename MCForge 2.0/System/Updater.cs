/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/24/2012
 * Time: 4:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MCForge.Utils.Settings;

namespace MCForge.Core
{
	/// <summary>
	/// The Updater
	/// </summary>
	public static class Updater
	{
		
		/// <summary>
		/// If true, commands, plugins, and the core will automatically update WITH notification
		/// </summary>
		public static bool autoupdate {
			get {
				return bool.Parse(ServerSettings.GetSetting("auto-update"));
			}
		}
		
		/// <summary>
		/// If true, commands and plugins will be updated without notification
		/// </summary>
		public static bool silentupdate {
			get {
				return bool.Parse(ServerSettings.GetSetting("silent-update"));
			}
		}
		
		/// <summary>
		/// If true, the server will ask before updating the core (If GUI is enabled)
		/// If no gui is enabled, then the user must update using /update
		/// If silentupdate or autoupdate is enabled, then this setting is ignored
		/// </summary>
		public static bool askbefore {
			get {
		        return bool.Parse(ServerSettings.GetSetting("ask-before-core"));
			}
		}
		
		/// <summary>
		/// If true, the server will ask before updating plugins and commands (If GUI is enabled)
		/// If no gui is enabled, then the user must update using /update
		/// If silentupdate or autoupdate is enabled, then this setting is ignored
		/// </summary>
		public static bool askbeforemisc {
			get {
		        return bool.Parse(ServerSettings.GetSetting("ask-before-misc"));
			}
		}
		
		/// <summary>
		/// If enabled, the server will attempt to udpate when server activity is low
		/// </summary>
		public static bool silentcoreupdate {
		    get {
		        return bool.Parse(ServerSettings.GetSetting("silent-core-update"));
		    }
		}
		
		/// <summary>
		/// How often to check for updates (in minutes)
		/// </summary>
		public static int checkinterval {
		    get {
		        return int.TryParse(ServerSettings.GetSetting("updatecheck-interval");
		    }
		}
		
		/// <summary>
        /// The server version
        /// </summary>
        public static Version Version {
        	get {
        		return Assembly.GetEntryAssembly().GetName().Version;
        	}
        }
        
        internal static void InIt() {
            
        }
	}
}
