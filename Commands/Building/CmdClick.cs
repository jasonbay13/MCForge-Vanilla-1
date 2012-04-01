using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MCForge;

namespace CommandDll
{
    public class CmdClick : ICommand
    {
        public string Name { get { return "Click"; } }
        public CommandTypes Type { get { return CommandTypes.building; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            //if (p == null) stuff here, not sure if should add yet or not since Console cannot do commands at the moment.
            Point3 click = p.lastClick;
            if (args.Length == 0)
            {
                click = p.lastClick;
            }
            else if (args.Length == 3)
            {
                try
                {
                    for (int value = 0; value < 3; value++)
                    {
                        switch (args[value].ToLower())
                        {
                            case "x":
                                click.x = p.lastClick.x;
                                break;
                            case "z":
                                click.z = p.lastClick.z;
                                break;
                            case "y":
                                click.y = p.lastClick.y;
                                break;
                            default:
                                if (isValid(args[value], value, p))
                                {
                                    switch (value)
                                    {
                                        case 0:
                                            click.x = short.Parse(args[0]);
                                            break;
                                        case 1:
                                            click.z = short.Parse(args[1]);
                                            break;
                                        case 2:
                                            click.y = short.Parse(args[2]);
                                            break;
                                    }
                                }
                                else
                                {
                                    p.SendMessage("\"" + args[value] + "\" was not valid");
                                    return;
                                }
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Server.Log(e);
                    return;
                }
            }
            else
            {
                p.SendMessage("Invalid arguments");
                return;
            }

            p.click((ushort)click.x, (ushort)click.z, (ushort)click.y, (byte)(Blocks.Types.stone));
            p.SendMessage("Click &b(" + click.x + ", " + click.z + ", " + click.y + ").");
        }

        public void Help(Player p)
        {
            p.SendMessage("/click [x z y]- Fakes a click");
            p.SendMessage("if no xyz is given, it uses the last place clicked.");
            p.SendMessage("/click 200 z 200 will cuase it to click at 200x, last z, and 200y");
        }

        private bool isValid(string message, int dimension, Player p)
        {
            ushort testValue;
            try
            {
                testValue = ushort.Parse(message);
            }
            catch
            {
                return false;
            }
            if (testValue < 0)
                return false;

            if (testValue >= p.level.Size.x && dimension == 0) return false;
            else if (testValue >= p.level.Size.z && dimension == 1) return false;
            else if (testValue >= p.level.Size.y && dimension == 2) return false;
            return true;
        }

        public void Initialize()
        {
            string[] CommandStrings = new string[2] { "click", "x" };
            Command.AddReference(this, CommandStrings);
        }
    }
}
