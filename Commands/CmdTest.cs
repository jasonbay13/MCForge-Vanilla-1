using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using McForge;

namespace CommandDll
{
	public class CmdTest : ICommand
	{
		string _Name = "test";
		public string Name { get { return _Name; } }

		CommandTypes _Type = CommandTypes.misc;
		public CommandTypes Type { get { return _Type; } }

		string _Author = "Merlin33069";
		public string Author { get { return _Author; } }

		int _Version = 1;
		public int Version { get { return _Version; } }

		string _CUD = "";
		public string CUD { get { return _CUD; } }

		string[] CommandStrings = new string[1] { "test" };

		public void Use(Player p, string[] args)
		{
			p.SendMessage("Please place/destroy a block.");
			p.CatchNextBlockchange(new Player.BlockChangeDelegate(BlockChange), null);
		}
		public void BlockChange(Player p, ushort x, ushort z, ushort y, byte NewType, bool action, object data)
		{
			//HandleBlockChange
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
