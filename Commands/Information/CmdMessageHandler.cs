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
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Interface.Plugin;
using MCForge.Utilities;

namespace CommandDll.Information
{
    public class CmdMessageHandler : ICommand
    {

        public string Name
        {
            get { return "MessageHandler"; }
        }

        public string[] CommandNames
        {
            get { return new string[] { "v" }; }// "stop", "1", "2", "3", "4", "5", "6", "7", "8", "9", "next", "np", "nl", "previous", "prev", "pp", "pl" }; }
        }

        public CommandTypes Type
        {
            get { return CommandTypes.information; }
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
            get { return ""; }
        }

        byte _Permission = 0;
        public byte Permission { get { return _Permission; } }

        public void Use(Player p, string[] args)
        {
            if (viewer == null)
            {
                Logger.Log("[CmdMessageHandler]: searching IPluginMessageHandler");
                viewer = (IPluginMessageViewer)Plugin.getByInterface("IPluginMessageViewer");
            }
            if (viewer == null)
            {
                Logger.Log("[CmdMessageHandler]: no IPluginMessageHanlder found!");
            }
            else
            {
                if (args.Length == 0)
                {
                    Help(p);
                    return;
                }
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
                    case "n":
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
                    case "p":
                    case "pp": viewer.ShowPreviousPage(p); break;
                    case "pl": viewer.ShowPreviousLine(p); break;
                    default: break;
                }
            }
        }

        //"stop", "1", "2", "3", "4", "5", "6", "7", "8", "9", "next", "np", "nl", "previous", "prev", "pp", "pl" }; }
        public void Help(Player p)
        {
            string message = "%eUse /v %1stop%e to stop reading.\n" +
                "Use /v %11%e, %12%e, %13%e, %14%e, %15%e, %16%e, %17%e, %18%e or %19%e to switch to a page.\n" +
                "Use /v %1next%e, %1next page%e, %1np%e or %1n%e to switch to next page.\n" +
                "Use /v %1previous%e, %1previous page%e, %1prev%e, %1prev page%e, %1pp%e or %1p%e to switch to previous page.\n" +
                "Use /v %1next line%e or %1nl%e to show next line.\n" +
                "Use /v %1previous%e line, %1prev%e line or pl to show previous line.\n" +
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
            Command.AddReference(this, CommandNames);
        }
    }
}