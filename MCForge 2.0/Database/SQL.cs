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
	public abstract class ISQL : IDisposable
	{
		protected bool _disposed;
		public virtual void executeQuery(string queryString) {}
		public virtual void executeQuery(string[] queryString) {}
		public virtual void onLoad() { }
		public virtual DataTable fillData(string queryString) { return null; }
		public virtual void Dispose()
		{
			if (!_disposed)
			{
				//TODO Dispose shit here
				_disposed = true;
			}
		}
	}
}
