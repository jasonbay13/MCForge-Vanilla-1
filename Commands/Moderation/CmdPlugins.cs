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
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Interface.Plugin;

namespace CommandDll.Moderation
{
    public class CmdPlugins : ICommand
    {
        public string Name { get { return "plugins"; } }
        public CommandTypes Type { get { return CommandTypes.Mod; } }
        public string Author { get { return "ninedrafted"; } }
        public int Version { get { return 1; } }
        public string CUD { get { throw new NotImplementedException(); } }
        public byte Permission { get { return 0; } /*SuperOP 100*/ }
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
            p.SendMessage("/plugins show - Shows all loaded plugins");
            p.SendMessage("/plugins unload <name> - Unloads a plugin called [name] (ignores case)");
            p.SendMessage("/plugins load <name> - Tries to load a plugin with [name] (ignores case)");
            p.SendMessage("/plugins reload - Loads all unloaded plugins");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[] { "plugins" });
        }
    }
}
