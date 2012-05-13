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
using MCForge.Utils.Settings;
using MySql.Data.Types;
using MySql.Data.MySqlClient;
using MCForge.Utils;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of MySQL.
	/// </summary>
	internal class MySQL : ISQL, IDisposable
	{
		public string connString = "";
		protected MySqlConnection conn;
		protected bool _closed = true;
		public override void onLoad()
		{
			connString = string.Format("Data Source={0};Port={1};User ID={2};Password={3};Pooling={4}", ServerSettings.GetSetting("MySQL-IP"), ServerSettings.GetSetting("MySQL-Port"), ServerSettings.GetSetting("MySQL-Username"), ServerSettings.GetSetting("MySQL-Password"), ServerSettings.GetSetting("MySQL-Pooling"));
			Open();
			executeQuery("CREATE DATABASE if not exists '" + ServerSettings.GetSetting("MySQL-DBName") + "'");
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
