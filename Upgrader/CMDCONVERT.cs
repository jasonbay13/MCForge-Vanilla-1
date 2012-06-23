/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/23/2012
 * Time: 6:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace MCForge_
{
	/// <summary>
	/// Description of CMDCONVERT.
	/// </summary>
	public class CMDCONVERT
	{
		public static void CONVERTCMD()
		{
			string[] lines = File.ReadAllLines("properties/command.properties");
			List<string> save = new List<string>();
			string cmd = "";
			foreach (string line in lines)
			{
				if (!line.StartsWith("#"))
				{
					try {
						cmd = line.Split(':')[0].Trim();
						int permission = int.Parse(line.Split(':')[1].Trim());
						save.Add(cmd + ":" + permission);
					}
					catch {
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Error getting permission for " + cmd);
						Console.ForegroundColor = ConsoleColor.Green;
					}
				}
			}
			File.WriteAllLines("properties/command.properties", save.ToArray());
			Console.WriteLine("Converted " + save.Count + " command permission settings!");
			save.Clear();
		}
	}
}
