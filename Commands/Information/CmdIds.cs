using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
	public class CmdIds : ICommand
	{
		string _Name = "PlayerIds";
		public string Name { get { return _Name; } }

		CommandTypes _Type = CommandTypes.information;
		public CommandTypes Type { get { return _Type; } }

		string _Author = "Merlin33069";
		public string Author { get { return _Author; } }

		int _Version = 1;
		public int Version { get { return _Version; } }

		string _CUD = "";
		public string CUD { get { return _CUD; } }

		string[] CommandStrings = new string[1] { "ids" };

		public void Use(Player p, string[] args)
		{
			foreach (Player pl in Server.Players.ToArray())
			{
				p.SendMessage(pl.USERNAME + " " + pl.id);
			}
		}

		public void Help(Player p)
		{

		}

		public void Initialize()
		{
			Command.AddReference(this, CommandStrings);
		}
	}
}
