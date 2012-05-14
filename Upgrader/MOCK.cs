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
			//TODO Start upgrade or start a seperate program to start update..
		}
	}
}