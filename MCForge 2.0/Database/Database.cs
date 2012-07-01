/*
Copyright 2012 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/
using System;
using System.Data;
using MCForge.Core;
using System.Collections.Generic;
using MCForge.Utils.Settings;
using System.Drawing;
using MCForge.Utils;
using System.Threading;
using System.Collections.Specialized;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of Database.
	/// </summary>
	public static class Database
	{
		static ISQL SQLType;
		//TODO Add Queue option..
		public static bool queuecommands { get { return bool.Parse(ServerSettings.GetSetting("Database-Queuing")); } }
		public static int FlushWait
		{
			get
			{
				try
				{
					return int.Parse(ServerSettings.GetSetting("Database-Flush_Interval"));
				}
				catch
				{
					return 1000;
				}
			}
		}
		private static Thread _worker;
		private static bool flushcommands;
		private static Queue<string> commands = new Queue<string>();
		public static ISQL SQL { get { return SQLType; } }
		internal static void init()
		{
			if (SQLType == null)
			{
				Logger.Log("Starting Database Service", LogType.Debug);
				switch (ServerSettings.GetSetting("DatabaseType"))
				{
					case "mysql":
						SQLType = new MySQL();
						break;
					case "sqlite":
						SQLType = new SQLite();
						break;
					default:
						Logger.Log("Database Type not found!",Color.Red, Color.Gray);
						Logger.Log("Using SQLite", Color.Green, Color.Gray);
						SQLType = new SQLite();
						break;
				}
				SQLType.onLoad();
			}
			if (_worker == null)
			{
				Logger.Log("Database Queuing starting", LogType.Debug);
				_worker = new Thread(Flush);
				flushcommands = true;
				_worker.Start();
			}
		}
		/// <summary>
		/// Add a sql command to the queue
		/// </summary>
		/// <param name="cmd">The command to add</param>
		public static void QueueCommand(string cmd)
		{
			commands.Enqueue(cmd);
		}
		
		/// <summary>
		/// Add multiple commands to the queue
		/// </summary>
		/// <param name="cmds">The array of commands to add</param>
		public static void QueueCommands(string[] cmds)
		{
			for (int i = 0; i < cmds.Length; i++)
			{
				QueueCommand(cmds[i]);
			}
		}
		
		/// <summary>
		/// Flush all the commands
		/// This will execute all the commands in the queue and remove them
		/// </summary>
		internal static void Flush()
		{
			while (flushcommands)
			{
                if (Server.ShuttingDown) return;
				Thread.Sleep(FlushWait);
                if (commands.Count > 0) {
                    string[] cmds;
                    lock (commands) {
                        Logger.Log("Flushing " + commands.Count + " commands");
                        cmds = commands.ToArray();
                        commands.Clear();
                    }
                    executeQuery(cmds);
                }
			}
		}
		
		/// <summary>
		/// Close the SQL connection and dispose resources
		/// THIS SHOULD ONLY BE CALLED ONCE
		/// </summary>
		public static void Dispose()
		{
			flushcommands = false;
			//Flush remaining commands
			while (commands.Count > 0)
				executeQuery(commands.Dequeue());
			SQLType.Dispose();
		}
		
		/// <summary>
		/// Execute a command to the sql server
		/// This wont return any data
		/// If the server is set to queue all commands, then the commands will be added to queue
		/// </summary>
		/// <param name="queryString">The command to execute</param>
		public static void executeQuery(string queryString)
		{
			//Logger.Log("Executing " + queryString, LogType.Normal);
			if (queuecommands)
				QueueCommand(queryString);
			else
				SQLType.executeQuery(queryString);
		}
		
		/// <summary>
		/// Execute multiple commands to the sql server
		/// This wont return any data
		/// If the server is set to queue all commands, then the commands will be added to queue
		/// </summary>
		/// <param name="commands">The commands to execute</param>
		public static void executeQuery(string[] commands)
		{
			Logger.Log("Executing " + commands.Length + " commands..", LogType.Debug);
            string cmds = String.Join("; ", commands);
				SQLType.executeQuery(cmds);
		}
		
		/// <summary>
		/// Execute a command to the sql server and return a datatable of the result
		/// </summary>
		/// <param name="queryString">The command to execute</param>
		/// <returns>The datatable</returns>
		public static DataTable fillData(string queryString)
		{
			Logger.Log("Executing " + queryString + " (FillData)", LogType.Debug);
			return SQLType.fillData(queryString);
		}
        public static IEnumerable<NameValueCollection> getData(string query) {
            foreach (NameValueCollection nvm in SQLType.getData(query)) {
                yield return nvm;
            }
        }
	}
}
