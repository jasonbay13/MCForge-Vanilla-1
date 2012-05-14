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
using MCForge.Utilities.Settings;

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
		}
		
		public override void executeQuery(string queryString)
		{
			using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn)) {
				cmd.ExecuteNonQuery();
			}
		}
		
		public override DataTable fillData(string queryString)
		{
			DataTable db = new DataTable("toReturn");
			using (SQLiteDataAdapter da = new SQLiteDataAdapter(queryString, conn)) {
				da.Fill(db);
			}
			return db;
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
