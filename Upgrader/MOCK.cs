/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 5/13/2012
 * Time: 10:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using MCForge;
using System.Data;
using MCForge.SQL;
using System.IO;

/// <summary>
/// This will be used to trick older version of MCForge to run the upgrader
/// That way we can execute a smooth update process using the old update system
/// </summary>
namespace MCForge_.Gui
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			if (Process.GetProcessesByName("MCForge").Length != 1)
			{
				foreach (Process pr in Process.GetProcessesByName("MCForge"))
				{
					if (pr.MainModule.BaseAddress == Process.GetCurrentProcess().MainModule.BaseAddress)
						if (pr.Id != Process.GetCurrentProcess().Id)
							pr.Kill();
				}
			}
			Console.WriteLine("Starting upgrade...");
			//TODO Start upgrade or start a seperate program to start update.
			Console.WriteLine("Loading old properties...");
			Server.s = new Server();
			Server.LoadAllSettings();
			Console.WriteLine("Converting database...");
			//Convert Database for MySQL users
			if (Server.useMySQL)
			{
				//Create the tables..
				MySQL.execute("CREATE TABLE if not exists _players (UID INTEGER not null auto_increment, Name VARCHAR(20), IP VARCHAR(20), firstlogin DATETIME, lastlogin DATETIME, money MEDIUMINT, totallogin MEDIUMINT, PRIMARY KEY (UID));");
				MySQL.execute("CREATE TABLE if not exists extra (key VARCHAR(1000), value VARCHAR(1000), UID INTEGER);");
				//Lets start with the players
				DataTable table = new DataTable("table");
				MySQL.fill("SELECT * FROM Players", table);
				for (int i = 0; i < table.Rows.Count; i++)
				{
					MySQL.execute("INSERT INTO _players (Name, IP, firstlogin, lastlogin, money, totallogin, totalblocks) VALUES ('" + table.Rows[i]["Name"].ToString() + "', '" + table.Rows[i]["IP"].ToString() + "', '" + table.Rows[i]["FirstLogin"].ToString() + "', '" + table.Rows[i]["LastLogin"].ToString() + "', " + int.Parse(table.Rows[i]["Money"].ToString()) + ", " + int.Parse(table.Rows[i]["totalLogin"].ToString()) + ", " + int.Parse(table.Rows[i]["totalBlocks"].ToString()) + ")");
					//Title and Color are to be treated as an extra item...
					MySQL.execute("INSERT INTO extra (key, value, UID) VALUES ('Title', '" + table.Rows[i]["Title"].ToString() + "', " + i + ")");
					MySQL.execute("INSERT INTO extra (key, value, UID) VALUES ('Color', '" + table.Rows[i]["color"].ToString() + "', " + i + ")");
				}
				//Were done
				table.Dispose();
				//DROP THE OLD TABLE!
				//Maybe ._.
				Console.WriteLine("Finished converting player table..");
				
				Console.WriteLine("Converting Level data..");
				MySQL.execute("CREATE TABLE if not exists Blocks (UID INTEGER, X MEDIUMINT, Y MEDIUMINT, Z MEDIUMINT, Level VARCHAR(100), Deleted VARCHAR(30), Date DATETIME);");
				foreach (string level in Directory.GetFiles("levels", "*.lvl"))
				{
					try {
						//Convert Block History
						string name = Path.GetFileName(level).Split('.')[0];
						DataTable table1 = new DataTable("table1");
						MySQL.fill("SELECT * FROM Block" + name, table1);
						DataTable temp = new DataTable("temp");
						for (int i = 0; i < table1.Rows.Count; i++)
						{
							//Convert username to UID
							MySQL.fill("SELECT * FROM Players WHERE Name='" + table1.Rows[i]["Username"].ToString() + "'", temp);
							int UID = int.Parse(temp.Rows[0]["UID"].ToString());
							int x = int.Parse(table1.Rows[i]["X"].ToString());
							int y = int.Parse(table1.Rows[i]["Y"].ToString());
							int z = int.Parse(table1.Rows[i]["Z"].ToString());
							string time = table1.Rows[i]["TimePerformed"].ToString();
							int deleted = int.Parse(table1.Rows[i]["deleted"].ToString());
							string finaldel = (deleted == 1 ? "true" : "false");
							MySQL.execute("INSERT INTO Blocks (UID, X, Y, Z, Level, Deleted, Date) VALUES (" + UID + ", " + x + ", " + y + ", " + z + ", '" + name + "', '" + finaldel + "', '" + time + "')");
						}
						//TODO Add zones, mb, and portals once those are in
					}
					catch { 
						Console.WriteLine("Converting failed for " + level);
					}
				}
				
			}
			else
			{
				SQLite.execute("CREATE TABLE if not exists _players (UID INTEGER not null PRIMARY KEY AUTOINCREMENT, Name VARCHAR(20), IP VARCHAR(20), firstlogin DATETIME, lastlogin DATETIME, money MEDIUMINT, totallogin MEDIUMINT);");
				SQLite.execute("CREATE TABLE if not exists extra (key VARCHAR(1000), value VARCHAR(1000), UID INTEGER);");
				DataTable table = new DataTable("table");
				SQLite.fill("SELECT * FROM Players", table);
				for (int i = 0; i < table.Rows.Count; i++)
				{
					SQLite.execute("INSERT INTO _players (Name, IP, firstlogin, lastlogin, money, totallogin, totalblocks) VALUES ('" + table.Rows[i]["Name"].ToString() + "', '" + table.Rows[i]["IP"].ToString() + "', '" + table.Rows[i]["FirstLogin"].ToString() + "', '" + table.Rows[i]["LastLogin"].ToString() + "', " + int.Parse(table.Rows[i]["Money"].ToString()) + ", " + int.Parse(table.Rows[i]["totalLogin"].ToString()) + ", " + int.Parse(table.Rows[i]["totalBlocks"].ToString()) + ")");
					//Title and Color are to be treated as an extra item...
					SQLite.execute("INSERT INTO extra (key, value, UID) VALUES ('Title', '" + table.Rows[i]["Title"].ToString() + "', " + i + ")");
					SQLite.execute("INSERT INTO extra (key, value, UID) VALUES ('Color', '" + table.Rows[i]["color"].ToString() + "', " + i + ")");
				}
				table.Dispose();
				Console.WriteLine("Finished converting player table..");
				
				Console.WriteLine("Converting Level data..");
				SQLite.execute("CREATE TABLE if not exists Blocks (UID INTEGER, X MEDIUMINT, Y MEDIUMINT, Z MEDIUMINT, Level VARCHAR(100), Deleted VARCHAR(30), Date DATETIME);");
				foreach (string level in Directory.GetFiles("levels", "*.lvl"))
				{
					try {
						//Convert Block History
						string name = Path.GetFileName(level).Split('.')[0];
						DataTable table1 = new DataTable("table1");
						SQLite.fill("SELECT * FROM Block" + name, table1);
						DataTable temp = new DataTable("temp");
						for (int i = 0; i < table1.Rows.Count; i++)
						{
							//Convert username to UID
							SQLite.fill("SELECT * FROM Players WHERE Name='" + table1.Rows[i]["Username"].ToString() + "'", temp);
							int UID = int.Parse(temp.Rows[0]["UID"].ToString());
							int x = int.Parse(table1.Rows[i]["X"].ToString());
							int y = int.Parse(table1.Rows[i]["Y"].ToString());
							int z = int.Parse(table1.Rows[i]["Z"].ToString());
							string time = table1.Rows[i]["TimePerformed"].ToString();
							int deleted = int.Parse(table1.Rows[i]["deleted"].ToString());
							string finaldel = (deleted == 1 ? "true" : "false");
							SQLite.execute("INSERT INTO Blocks (UID, X, Y, Z, Level, Deleted, Date) VALUES (" + UID + ", " + x + ", " + y + ", " + z + ", '" + name + "', '" + finaldel + "', '" + time + "')");
						}
						//TODO Add zones, mb, and portals once those are in
					}
					catch { 
						Console.WriteLine("Converting failed for " + level);
					}
				}
			}
		}
	}
}