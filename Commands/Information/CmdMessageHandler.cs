/*
Copyright 2011 MCForge
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
﻿using System;
using MCForge.Core;
using MCForge.Entity;
using MCForge.Interface.Command;
using MCForge.Interface.Plugin;

namespace CommandDll.Information
{
    class CmdMessageHandler : ICommand
    {

        public string Name { get { return "MessageHandler"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "ninedrafted"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 0; } }

        public void Use(Player p, string[] args)
        {
            if (viewer == null)
            {
                Server.Log("[CmdMessageHandler]: searching IPluginMessageHandler");
                viewer = (IPluginMessageViewer)PluginManager.getByInterface("IPluginMessageViewer");
            }
            if (viewer == null)
            {
                Server.Log("[CmdMessageHandler]: no IPluginMessageHanlder found!");
            }
            else
            {
                switch (args[0])
                {
                    case "stop": viewer.Stop(p); break;
                    case "1": viewer.ShowPage(p, 1); break;
                    case "2": viewer.ShowPage(p, 2); break;
                    case "3": viewer.ShowPage(p, 3); break;
                    case "4": viewer.ShowPage(p, 4); break;
                    case "5": viewer.ShowPage(p, 5); break;
                    case "6": viewer.ShowPage(p, 6); break;
                    case "7": viewer.ShowPage(p, 7); break;
                    case "8": viewer.ShowPage(p, 8); break;
                    case "9": viewer.ShowPage(p, 9); break;
                    case "next":
                        if (args.Length > 1)
                        {
                            if (args[1] == "line")
                                viewer.ShowNextLine(p);
                            else
                                viewer.ShowNextPage(p);
                        }
                        else
                            viewer.ShowNextPage(p);
                        break;
                    case "np": viewer.ShowNextPage(p); break;
                    case "nl": viewer.ShowNextLine(p); break;
                    case "previous":
                    case "prev":
                        if (args.Length > 1)
                        {
                            if (args[1] == "line")
                                viewer.ShowPreviousLine(p);
                            else
                                viewer.ShowPreviousPage(p);
                        }
                        else
                            viewer.ShowPreviousPage(p);
                        break;
                    case "pp": viewer.ShowPreviousPage(p); break;
                    case "pl": viewer.ShowPreviousLine(p); break; //will mess with /place->/pl
                    default: break;
                }
            }
        }
        //"stop", "1", "2", "3", "4", "5", "6", "7", "8", "9", "next", "np", "nl", "previous", "prev", "pp", "pl" }; }
        public void Help(Player p)
        {
            string message = "%eUse %1/stop%e to stop reading.\n" +
                "Use %1/1%e, %1/2%e, %1/3%e, %1/4%e, %1/5%e, %1/6%e, %1/7%e, %1/8%e or %1/9%e to switch to a page.\n" +
                "Use %1/next%e, %1/next page%e or %1/np%e to switch to next page.\n" +
                "Use %1/previous%e, %1/previous page%e, %1/prev%e, %1/prev page%e or %1/pp%e to switch to previous page.\n" +
                "Use %1/next line%e or %1/nl%e to show next line.\n" +
                "Use %1/previous%e line, %1/prev%e line or /pl to show previous line.\n" +
                "\n" +
                "This help is shown through PluginMessageViewer. As the Plugin is not yet finished this help will be messing up with chat messages." +
                " In coming releases it will append all chat messages at the end of the text, so nothing gets missed.\n" +
                "\n" +
                "\nThis is the last line.";
            viewer.ShowMessage(p, message);

        }
        IPluginMessageViewer viewer;
        public void Initialize()
        {
            Command.AddReference(this, new string[17] {"stop", "1", "2", "3", "4", "5", "6", "7", "8", "9", "next", "np", "nl", "previous", "prev", "pp", "pl"});
        }
    }
}