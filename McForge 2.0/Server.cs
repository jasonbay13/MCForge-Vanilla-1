/*
Copyright 2011 MCForge
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Timers;

namespace McForge
{
	public static class Server
	{
		public static int port = 25565;

		public static bool shuttingDown;
		public static bool Started = false;
		public static System.Timers.Timer UpdateTimer;

		public static List<Player> Connections = new List<Player>();
		public static List<Player> Players = new List<Player>();
		public static List<string> BannedIP = new List<string>();

		public static Level Mainlevel;

		public static void Init()
		{
			StartListening();

			Mainlevel = Level.CreateLevel(new Point3(256, 256, 64), Level.LevelTypes.Flat);


			UpdateTimer = new System.Timers.Timer(100);
			UpdateTimer.Elapsed += delegate { Update(); };
			UpdateTimer.Start();

			LoadAllDlls.Init();

			Log("[Important]: Server Started.", ConsoleColor.Black, ConsoleColor.White);
			Started = true;
		}

		static void Update()
		{
			Player.GlobalUpdate();
		}

		#region Socket Stuff
		public static TcpListener listener;
		public static void StartListening()
		{
			startretry:
			try
			{
				listener = new TcpListener(System.Net.IPAddress.Any, 25565);
				listener.Start();
				IAsyncResult ar = listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
			}
			catch (Exception E)
			{
				Server.Log(E);
				goto startretry;
			}
		}
		public static void AcceptCallback(IAsyncResult ar)
		{
			TcpListener listener2 = (TcpListener)ar.AsyncState;
			try
			{
				TcpClient clientSocket = listener2.EndAcceptTcpClient(ar);
				new Player(clientSocket);
			}
			catch { }
			if (!shuttingDown)
			{
				listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), listener);
			}
		}
		#endregion
		#region Log Stuff
		/// <summary>
		/// Write A message to the Console and the GuiLog using default (white on black) colors.
		/// </summary>
		/// <param name="message">The message to show</param>
		public static void Log(string message)
		{
			Log(message, ConsoleColor.White, ConsoleColor.Black);
		}
		/// <summary>
		/// Write an error to the Console and the GuiLog using Red on black colors
		/// </summary>
		/// <param name="E">The error exception to write.</param>
		public static void Log(Exception E)
		{
			Log("[ERROR]: ", ConsoleColor.Red, ConsoleColor.Black);
			Log(E.Message, ConsoleColor.Red, ConsoleColor.Black);
			Log(E.StackTrace, ConsoleColor.Red, ConsoleColor.Black);
		}
		/// <summary>
		/// Write a message to the console and GuiLog using a specified TextColor and BackGround Color
		/// </summary>
		/// <param name="message">The Message to show</param>
		/// <param name="TextColor">The color of the text to show</param>
		/// <param name="BackgroundColor">The color behind the text.</param>
		public static void Log(string message, ConsoleColor TextColor, ConsoleColor BackgroundColor)
		{
			Console.ForegroundColor = TextColor;
			Console.BackgroundColor = BackgroundColor;
			Console.WriteLine(message.PadRight(Console.WindowWidth - 1));
			Console.ResetColor();
		}
		#endregion
	}
}
