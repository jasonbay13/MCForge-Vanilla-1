using System;
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;

namespace CommandDll
{
	public class CmdIds : ICommand
	{
        public string Name { get { return "PlayerIds"; } }
		public CommandTypes Type { get { return CommandTypes.Information; } }
        public string Author { get { return "Merlin33069"; } }
		public Version Version { get { return new Version(1,0); } }
		public string CUD { get { return ""; } }
        public byte Permission { get { return 120; } }
		string[] CommandStrings = new string[1] { "ids" };
		public void Use(Player p, string[] args)
		{
			Server.ForeachPlayer(delegate(Player pl)
			{
				p.SendMessage(pl.Username + " " + pl.id);
			});
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
