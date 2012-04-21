/*
 * Created by SharpDevelop.
 * User: Eddie
 * Date: 4/15/2012
 * Time: 11:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;

namespace MCForge.SQL
{
	/// <summary>
	/// Description of SQL.
	/// </summary>
	public abstract class ISQL
	{
		public abstract void executeQuery(string queryString);
		public abstract void executeQuery(string[] queryString);
		public abstract void onLoad();
		public abstract DataTable fillData(string queryString);
	}
}
