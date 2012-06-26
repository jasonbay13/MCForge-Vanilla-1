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
using MCForge.Utils.Settings;
using MySql.Data.Types;
using MySql.Data.MySqlClient;
using MCForge.Utils;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of MySQL.
	/// </summary>
	internal class MySQL : ISQL
	{
		public string connString = "";
		protected MySqlConnection conn;
		protected bool _closed = true;
		public override void onLoad()
		{
			connString = string.Format("Data Source={0};Port={1};User ID={2};Password={3};Pooling={4}", ServerSettings.GetSetting("MySQL-IP"), ServerSettings.GetSetting("MySQL-Port"), ServerSettings.GetSetting("MySQL-Username"), ServerSettings.GetSetting("MySQL-Password"), ServerSettings.GetSetting("MySQL-Pooling"));
			Open();
			string[] commands = new string[4];
			commands[0] = "CREATE DATABASE if not exists `" + ServerSettings.GetSetting("MySQL-DBName") + "`";
			commands[1] = "CREATE TABLE if not exists _players (UID INTEGER not null auto_increment, Name VARCHAR(20), IP VARCHAR(20), firstlogin DATETIME, lastlogin DATETIME, money MEDIUMINT, totallogin MEDIUMINT, totalblocks MEDIUMINT, color VARCHAR(5) PRIMARY KEY (UID));";
			commands[2] = "CREATE TABLE if not exists extra (key VARCHAR(1000), value VARCHAR(1000), UID INTEGER);";
			commands[3] = "CREATE TABLE if not exists Blocks (UID INTEGER, X MEDIUMINT, Y MEDIUMINT, Z MEDIUMINT, Level VARCHAR(100), Deleted VARCHAR(30), Block TEXT, Date DATETIME, Was TEXT);";
			executeQuery(commands);
		} 
		/// <summary>
		/// execute a query
		/// </summary>
		/// <param name="queryString">The command to execute</param>
		public override void executeQuery(string queryString)
		{
			try {
				if (queryString.IndexOf("CREATE DATABASE") != -1)
					conn.ChangeDatabase(ServerSettings.GetSetting("MySQL-DBName"));
				MySqlCommand cmd = new MySqlCommand(queryString, conn);
				cmd.ExecuteNonQuery();
			}
			catch (Exception e)
			{
                Logger.LogError(e);
			}
		}
		/// <summary>
		/// Execute more than 1 command at once
		/// Use this in a loop or if your going to execute more than 1 thing at a time
		/// </summary>
		/// <param name="queryString">The commands to execute</param>
		public override void executeQuery(string[] queryString)
		{
			try {
				for (int i = 0; i < queryString.Length; i++) {
					using (MySqlCommand cmd = new MySqlCommand(queryString[i], conn))
						cmd.ExecuteNonQuery();
				}
			}
			catch (Exception e)
			{
                Logger.LogError(e);
			}
		}
		
		public override DataTable fillData(string queryString)
		{
			DataTable db = new DataTable("toReturn");
			using (MySqlDataAdapter da = new MySqlDataAdapter(queryString, conn)) {
				da.Fill(db);
			}
			return db;
		}
		
		public void Open()
		{
			if (_closed)
			{
				conn = new MySqlConnection(connString);
				conn.Open();
				_closed = false;
			}
		}
		
		public void Close(bool dispose)
		{
			if (!_closed)
			{
				conn.Close();
				if (dispose)
					conn.Dispose();
				_closed = true;
			}
		}
		
		public override void Dispose()
		{
			if (!_disposed)
			{
				Close(true);
				base.Dispose();
			}
		}
	}
}
