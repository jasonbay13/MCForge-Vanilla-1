/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 4/15/2012
 * Time: 10:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using MCForge.Utils.Settings;
using MCForge.Utils;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of SQLite.
	/// </summary>
	internal class SQLite : ISQL
	{
		protected string connString;
		
		protected SQLiteConnection conn;
		protected bool _closed = true;
		public override void onLoad()
		{
			connString = "Data Source =" + Application.StartupPath + "/" + ServerSettings.GetSetting("SQLite-Filepath") + "; Version =3; Pooling =" + ServerSettings.GetSetting("SQLite-Pooling") + "; Max Pool Size =1000;";
			Open();
			string[] commands = new string[3];
			commands[0] = "CREATE TABLE if not exists _players (UID INTEGER not null PRIMARY KEY AUTOINCREMENT, Name VARCHAR(20), IP VARCHAR(20), firstlogin DATETIME, lastlogin DATETIME, money MEDIUMINT, totallogin MEDIUMINT);";
			commands[1] = "CREATE TABLE if not exists extra (key VARCHAR(1000), value VARCHAR(1000), UID INTEGER);";
			commands[2] = "CREATE TABLE if not exists Blocks (UID INTEGER, X MEDIUMINT, Y MEDIUMINT, Z MEDIUMINT, Level VARCHAR(100), Deleted VARCHAR(30));";
			executeQuery(commands);
		}
		public override void executeQuery(string[] queryString)
		{
			try {
				for (int i = 0; i < queryString.Length; i++)
				{
					using (SQLiteCommand cmd = new SQLiteCommand(queryString[i], conn)) {
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				Logger.LogError(e);
				Logger.Log("Error in SQLite..", LogType.Critical);
				Logger.Log("" + e);
			}
		}
		public override void executeQuery(string queryString)
		{
			try {
				using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn)) {
					cmd.ExecuteNonQuery();
				}
			}
			catch (Exception e)
			{
				Logger.LogError(e);
				Logger.Log("Error in SQLite..", LogType.Critical);
				Logger.Log("" + e);
			}
		}
		
		public override DataTable fillData(string queryString)
		{
			DataTable db = new DataTable("toReturn");
			try {
				using (SQLiteDataAdapter da = new SQLiteDataAdapter(queryString, conn)) {
					da.Fill(db);
				}
				return db;
			}
			catch (Exception e)
			{
				Logger.LogError(e);
				Logger.Log("Error in SQLite..", LogType.Critical);
				Logger.Log("" + e);
				return db;
			}
		}
		
		public void Open()
		{
			if (_closed)
			{
				conn = new SQLiteConnection(connString);
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
