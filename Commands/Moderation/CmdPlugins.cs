using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Interface.Plugin;
using MCForge.Entity;

namespace CommandDll.Moderation
{
    public class CmdPlugins:ICommand
    {
        public string Name
        {
            get { return "plugins"; }
        }

        public CommandTypes Type
        {
            get { return CommandTypes.Mod; }
        }

        public string Author
        {
            get { return "ninedrafted"; }
        }

        public int Version
        {
            get { return 1; }
        }

        public string CUD
        {
            get { throw new NotImplementedException(); }
        }

        public byte Permission
        {
            get { return 0; } //SuperOP 100
        }

        public void Use(Player p, string[] args)
        {
            if (args.Length == 0)
            {
                Help(p);
                return;
            }
            else
            {
                if (args.Length == 1)
                {
                    if (args[0] == "unload" || args[0] == "load")
                    {
                        p.SendMessage("Please specify a name");
                        return;
                    }
                    if (args[0] == "show")
                    {
                        string[] names = Plugin.GetNames();
                        if (names.Length > 0)
                        {
                            for (int i = 1; i < names.Length; i++)
                            {
                                names[0] += ", " + names[i];
                            }
                            p.SendMessage("Loaded plugins: " + names[0]);
                        }
                        else
                        {
                            p.SendMessage("No plugins loaded");
                        }
                        return;
                    }
                    if (args[0] == "reload")
                    {
                        int count = Plugin.reload();
                        p.SendMessage(count + " plugins loaded.");
                        return;
                    }
                    
                }
                if (args.Length == 2)
                {
                    if (args[0] == "unload")
                    {
                        if (Plugin.unload(args[1]))
                        {
                            p.SendMessage("Plugin " + args[1] + " unloaded");
                        }
                        else
                        {
                            p.SendMessage("No plugin " + args[1] + "unloaded");
                        }
                        return;
                    }
                    if (args[0] == "load")
                    {
                        try
                        {
                            int count = Plugin.reload(args[1]);
                            p.SendMessage(count + " plugins loaded.");
                            return;
                        }
                        catch { p.SendMessage("This plugin cannot be loaded"); return; }
                    }
                }
            }
            Help(p);
        }

        public void Help(Player p)
        {
            p.SendMessage("/plugins show: Shows all loaded plugins");
            p.SendMessage("/plugins unload [name]: Unloads a plugin called [name] (ignores case)");
            p.SendMessage("/plugins load [name]: Tries to load a plugin with [name] (ignores case)");
            p.SendMessage("/plugins reload: Loads all unloaded plugins");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[] { "plugins" });
        }
    }
}
