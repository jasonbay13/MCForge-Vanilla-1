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
using MCForge.Utilities.Settings;
using System.Drawing;
using MCForge.Utilities;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of Database.
	/// </summary>
	public static class Database
	{
		static ISQL SQLType;
		//TODO Add Queue option..
		public static ISQL SQL { get { return SQLType; } }
		internal static void init()
		{
			if (SQLType != null)
			{
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
		}
		public static void Dispose()
		{
			SQLType.Dispose();
		}
		public static void executeQuery(string queryString)
		{
			SQLType.executeQuery(queryString);
		}
		public static void executeQuery(string[] commands)
		{
			SQLType.executeQuery(commands);
		}
		public static DataTable fillData(string queryString)
		{
			return SQLType.fillData(queryString);
		}
	}
}
