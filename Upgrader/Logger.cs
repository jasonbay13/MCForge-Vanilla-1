/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 6/23/2012
 * Time: 6:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace MCForge
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class Logger
	{
		public static void Log(string message)
		{
			Console.WriteLine(message);
		}
		public static void Log(string message, System.Drawing.Color c, System.Drawing.Color cc)
		{
			Console.WriteLine(message);
		}
	}
}
