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
using MCForge_;
using System.Data;
using MCForge_.SQL;
using System.IO;
using MCForge.World;
using MCForge.Groups;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

/// <summary>
/// This will be used to trick older version of MCForge to run the upgrader
/// That way we can execute a smooth update process using the old update system
/// </summary>
namespace MCForge_.Gui
{
    public static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        public static void ThreadExHandler(object sender, ThreadExceptionEventArgs e)
        {
            Exception exception = e.Exception;
            Console.WriteLine(exception.ToString());
            Thread.Sleep(500);
        }
        public static void GlobalExHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine(ex.ToString());
            Thread.Sleep(500);
        }
        public static void Main(string[] args)
        {
            /*if (Process.GetProcessesByName("MCForge").Length != 1)
			{
				foreach (Process pr in Process.GetProcessesByName("MCForge"))
				{
					if (pr.MainModule.BaseAddress == Process.GetCurrentProcess().MainModule.BaseAddress)
						if (pr.Id != Process.GetCurrentProcess().Id)
							pr.Kill();
				}
			}*/
            //Get a pointer to the forground window.  The idea here is that
            //IF the user is starting our application from an existing console
            //shell, that shell will be the uppermost window.  We'll get it
            //and attach to it
            IntPtr ptr = GetForegroundWindow();

            int  u;

            GetWindowThreadProcessId(ptr, out u);

            Process process = Process.GetProcessById(u);

            if (process.ProcessName == "cmd" )    //Is the uppermost window a cmd process?
            {
                AttachConsole(process.Id);

                //we have a console to attach to ..
                Console.WriteLine("hello. It looks like you started me from an existing console.");
            }
            else
            {
                //no console AND we're in console mode ... create a new console.

                AllocConsole();
            }

            //FreeConsole();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.GlobalExHandler);
            Application.ThreadException += new ThreadExceptionEventHandler(Program.ThreadExHandler);
            try {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Starting upgrade...");
                //TODO Start upgrade or start a seperate program to start update.
                Console.WriteLine("Loading old properties...");
                Server.s = new Server();
                Server.LoadAllSettings();
                Console.WriteLine("Converting database...");
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            try {
                //Convert Database for MySQL users
                if (Server.useMySQL)
                {
                    //Create the tables..
                    MySQL.execute("CREATE TABLE if not exists _players (UID INTEGER not null auto_increment, Name VARCHAR(20), IP VARCHAR(20), firstlogin DATETIME, lastlogin DATETIME, money MEDIUMINT, totallogin MEDIUMINT, totalblocks MEDIUMINT, color VARCHAR(5) PRIMARY KEY (UID));");
                    MySQL.execute("CREATE TABLE if not exists extra (key VARCHAR(1000), value VARCHAR(1000), UID INTEGER);");
                    //Lets start with the players
                    DataTable table = new DataTable("table");
                    MySQL.fill("SELECT * FROM Players", table);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        try {
                            MySQL.execute("INSERT INTO _players (Name, IP, firstlogin, lastlogin, money, totallogin, totalblocks, color) VALUES ('" + table.Rows[i]["Name"].ToString() + "', '" + table.Rows[i]["IP"].ToString() + "', '" + table.Rows[i]["FirstLogin"].ToString() + "', '" + table.Rows[i]["LastLogin"].ToString() + "', " + int.Parse(table.Rows[i]["Money"].ToString()) + ", " + int.Parse(table.Rows[i]["totalLogin"].ToString()) + ", " + int.Parse(table.Rows[i]["totalBlocks"].ToString()) + ", '" + table.Rows[i]["color"].ToString() + "')");
                            //Title and Color are to be treated as an extra item...
                            MySQL.execute("INSERT INTO extra (key, value, UID) VALUES ('Title', '" + table.Rows[i]["Title"].ToString() + "', " + i + ")");
                            //MySQL.execute("INSERT INTO extra (key, value, UID) VALUES ('Color', '" + table.Rows[i]["color"].ToString() + "', " + i + ")");
                        }
                        catch {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Converting failed for column " + i);
                        }
                    }
                    //Were done
                    table.Dispose();
                    //DROP THE OLD TABLE!
                    //Maybe ._.
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Finished converting player table..");
                    
                    Console.WriteLine("Converting Level data..");
                    MySQL.execute("CREATE TABLE if not exists Blocks (UID INTEGER, X MEDIUMINT, Y MEDIUMINT, Z MEDIUMINT, Level VARCHAR(100), Deleted VARCHAR(30), Block TEXT, Date DATETIME, Was TEXT);");
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
                                MySQL.fill("SELECT * FROM _players WHERE Name='" + table1.Rows[i]["Username"].ToString() + "'", temp);
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
                            //Snowl panned them to be in the Level.ExtraData
                            //(it allows for portability of the .lvl so you can move it from server to server without requiring MySQL)
                        }
                        catch {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Converting failed for " + level);
                        }
                    }
                    
                }
                else
                {
                    try {
                        SQLite.execute("CREATE TABLE if not exists _players (UID INTEGER not null PRIMARY KEY AUTOINCREMENT, Name VARCHAR(20), IP VARCHAR(20), firstlogin DATETIME, lastlogin DATETIME, money MEDIUMINT, totallogin MEDIUMINT, totalblocks MEDIUMINT, color VARCHAR(5));");
                        SQLite.execute("CREATE TABLE if not exists extra (key VARCHAR(1000), value VARCHAR(1000), UID INTEGER);");
                        DataTable table = new DataTable("table");
                        SQLite.fill("SELECT * FROM Players", table);
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            SQLite.execute("INSERT INTO _players (Name, IP, firstlogin, lastlogin, money, totallogin, totalblocks, color) VALUES ('" + table.Rows[i]["Name"].ToString() + "', '" + table.Rows[i]["IP"].ToString() + "', '" + table.Rows[i]["FirstLogin"].ToString() + "', '" + table.Rows[i]["LastLogin"].ToString() + "', " + int.Parse(table.Rows[i]["Money"].ToString()) + ", " + int.Parse(table.Rows[i]["totalLogin"].ToString()) + ", " + int.Parse(table.Rows[i]["totalBlocks"].ToString()) + ", '" + table.Rows[i]["color"].ToString() + "')");
                            //Title and Color are to be treated as an extra item...
                            SQLite.execute("INSERT INTO extra (key, value, UID) VALUES ('Title', '" + table.Rows[i]["Title"].ToString() + "', " + i + ")");
                            //SQLite.execute("INSERT INTO extra (key, value, UID) VALUES ('Color', '" + table.Rows[i]["color"].ToString() + "', " + i + ")");
                        }
                        table.Dispose();
                        Console.WriteLine("Finished converting player table..");
                        
                        Console.WriteLine("Converting Level data..");
                        SQLite.execute("CREATE TABLE if not exists Blocks (UID INTEGER, X MEDIUMINT, Y MEDIUMINT, Z MEDIUMINT, Level VARCHAR(100), Deleted VARCHAR(30), Block TEXT, Date DATETIME, Was TEXT);");
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
                                    SQLite.fill("SELECT * FROM _players WHERE Name='" + table1.Rows[i]["Username"].ToString() + "'", temp);
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
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Converting failed for " + level);
                            }
                        }
                    }
                    catch {
                        Console.WriteLine("Error converting database, assuming there is none..");
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (string level in Directory.GetFiles("levels", "*.lvl"))
                {
                    try {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Level l = Level.LoadLevel(level);
                        if (l != null)
                        {
                            l.SaveToBinary();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(level + " converted!");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Level didnt load, must be a MCForge 6 level..");
                        }
                    }
                    catch (Exception e) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error converting " + level);
                        Console.WriteLine(e.ToString());
                    }
                    
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Converting Properties..");
                SrvProperties.ConvertSettings();
                Console.WriteLine("Converting Groups..");
                try {
                    Group.InitAll();
                    foreach (Group g in Group.GroupList) {
                        try {
                            PlayerGroup pg = new PlayerGroup((int)g.Permission, g.name, g.color, g.fileName);
                            pg.SaveGroup();
                            Console.WriteLine("Group " + g.name + " converted!");
                            PlayerGroup.Groups.Add(pg);
                        }
                        catch (Exception e) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error converting the group " + g.name + " !");
                            Console.WriteLine(e.ToString());
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                    }
                    Console.WriteLine("Saving new Groups..");
                    PlayerGroupProperties.Save();
                    File.Delete("properties/ranks.properties");
                    Console.WriteLine("Converting Command Permissions..");
                    CMDCONVERT.CONVERTCMD();
                    
                }
                catch (Exception e) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error converting groups!");
                    Console.WriteLine(e.ToString());
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("Renaming files..");
                File.Move("ranks/uberOps.txt", "ranks/superops.txt");
                File.Move("ranks/operators.txt", "ranks/ops.txt");
                Console.WriteLine("Moving Files..");
                string directory = "MCForge";
                if (!Directory.Exists("MCForge"))
                    Directory.CreateDirectory("MCForge");
                else {
                    int i = 0;
                    while (Directory.Exists("MCForge" + i))
                        i++;
                    Directory.CreateDirectory("MCForge" + i);
                    directory = "MCForge" + i;
                }
                Console.WriteLine("Moving Properties..");
                try { Directory.Move("properties", directory + "/properties"); } catch(Exception e) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error moving properties, you might need to do this after the upgrade!"); Console.ForegroundColor = ConsoleColor.Green; }
                try { Console.WriteLine("Moving Levels (This could take a while).."); } catch(Exception e) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error moving levels, you might need to do this after the upgrade!"); Console.ForegroundColor = ConsoleColor.Green; }
                Directory.Move("levels", directory + "/levels");
                if (!Server.useMySQL) {
                    Console.WriteLine("Moving SQLite DB..");
                    try { File.Copy(Server.apppath + "/MCForge.db", directory + "/MCForge.db"); } catch(Exception e) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error copying database, you might need to do this after the upgrade!"); Console.ForegroundColor = ConsoleColor.Green; }
                }
                Console.WriteLine("Moving ranks..");
                try { Directory.Move("ranks", directory + "/ranks"); } catch(Exception e) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error moving ranks, you might need to do this after the upgrade!"); Console.ForegroundColor = ConsoleColor.Green; }
                Console.WriteLine("Moving text..");
                try { Directory.Move("text", directory + "/text");  } catch(Exception e) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error moving text, you might need to do this after the upgrade!"); Console.ForegroundColor = ConsoleColor.Green; }
                Console.WriteLine("Archiving logs..");
                try { Directory.Move("logs", directory + "/old_logs");  } catch(Exception e) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error archiving logs, you might need to do this after the upgrade!"); Console.ForegroundColor = ConsoleColor.Green; }
                Console.Write("Downloading Core Commands..");
                MCForge.Downloader d = new MCForge.Downloader();
                d.Download("http://update.mcforge.net/DLL/Commands.dll", directory + "/Command.dll");
                d.Wait();
                Console.Write("\nDone!");
                d = new MCForge.Downloader();
                Console.Write("\nDownloading Core Plugins..");
                d.Download("http://update.mcforge.net/DLL/Plugins.dll", directory + "/Plugins.dll");
                d.Wait();
                Console.WriteLine("\nDone!");
                Console.Write("\nDownloading the ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("CORE");
                Console.ForegroundColor = ConsoleColor.Green;
                d = new MCForge.Downloader();
                d.Download("http://update.mcforge.net/DLL/Core.dll", directory + "/MCForge.dll");
                d.Wait();
                Console.Write("\nDone!\n");
                Console.WriteLine("Downloading Dependencies..");
                string[] todl = new string[0];
                using (WebClient wc = new WebClient())
                    todl = wc.DownloadString("http://update.mcforge.net/Dependencies/files.txt").Split(':');
                d = new MCForge.Downloader();
                foreach (string s in todl) {
                    Console.Write("\nDownloading " + s + "..");
                    d.Download("http://update.mcforge.net/Dependencies/" + s, directory + "/" + s);
                    d.Wait();
                    d.Reset();
                    Thread.Sleep(2000);
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); Console.ReadKey(true); }
            Console.ReadKey(true);
        }
    }
}