/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 4/15/2012
 * Time: 10:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MCForge.Core;
using MCForge.Utilities.Settings;
using MySql.Data.Types;
using MySql.Data.MySqlClient;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of MySQL.
	/// </summary>
	internal class MySQL : ISQL
	{
		public string connString = "";
		public override void onLoad()
		{
			connString = string.Format("Data Source={0};Port={1};User ID={2};Password={3};Pooling={4}", ServerSettings.GetSetting("MySQL-IP"), ServerSettings.GetSetting("MySQL-Port"), ServerSettings.GetSetting("MySQL-Username"), ServerSettings.GetSetting("MySQL-Password"), ServerSettings.GetSetting("MySQL-Pooling"));
			executeQuery("CREATE DATABASE if not exists '" + ServerSettings.GetSetting("MySQL-DBName") + "'");
		}
		public override void executeQuery(string queryString)
		{
			try {
				using (var conn = new MySqlConnection(connString))
				{
					conn.Open();
					if (queryString.IndexOf("CREATE DATABASE") != -1)
						conn.ChangeDatabase(ServerSettings.GetSetting("MySQL-DBName"));
					MySqlCommand cmd = new MySqlCommand(queryString, conn);
					cmd.ExecuteNonQuery();
					conn.Clone();
					conn.Dispose();
				}
			}
			catch (Exception e)
			{
				Server.Log(e);
			}
		}
		/*public override void executeQuery(string[] queryString) //TODO: Find out what this does
		{
			try {
				using (var conn = new MySqlConnection(connString))
				{
					conn.Open();
					if (queryString.IndexOf("CREATE DATABASE") != -1)
						conn.ChangeDatabase(ServerSettings.GetSetting("MySQL-DBName"));
					for (int i = 0; i < queryString.Length; i++) {
						using (MySqlCommand cmd = new MySqlCommand(queryString, conn))
							cmd.ExecuteNonQuery();
					}
					conn.Clone();
					conn.Dispose();
				}
			}
			catch (Exception e)
			{
				Server.Log(e);
			}
		}*/
	}
}
