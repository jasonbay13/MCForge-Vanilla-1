/*
Copyright 2012 MCForge
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
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

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
        public virtual IEnumerable<NameValueCollection> getData(string queryString) {
            yield return null;
        }
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
