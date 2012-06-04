using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interface.Command;
using MCForge.Groups;
using MCForge.API.Events;
using MCForge.Entity;
using MCForge.World;
using MCForge.Utils;
using MCForge.World.Drawing;
using System.Reflection;
using MCForge.Core;

namespace CommandDll.Building {
    public class CmdBrush : ICommand {

        #region ICommand Members

        public string Name {
            get { return "Brush"; }
        }

        public CommandTypes Type {
            get { return global::MCForge.Interface.Command.CommandTypes.Building; }
        }

        public string Author {
            get { return "MCForge Devs"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { throw new NotImplementedException(); }
        }

        public byte Permission {
            get { return (byte)PermissionLevel.AdvBuilder; }
        }

        public void Use(MCForge.Entity.Player p, string[] args) {
            if (args.Length < 1 || args.Length > 3) {
                Help(p);
                return;
            }

            if (args[0].ToLower() == "off" && p.ExtraData.ContainsKey("BrushData")) {
                p.OnPlayerBlockChange.Normal -= BlockChange;
                p.ExtraData.Remove("BrushData");
                p.SendMessage("Brush turned off");
                return;
            }

            byte block = 255;
            int size = 3;
            Type brushType =  Server.ServerAssembly.GetType("MCForge.World.Drawing." + StringUtils.CapitolizeFirstChar(args[0]) + "Brush");

            if (brushType == null) {
                p.SendMessage("Invalid brush type");
                Help(p);
                return;
            }

            if (args.Length == 3) {
                block = Block.NameToBlock(args[2]);
                if (block == Block.BlockList.UNKNOWN) {
                    p.SendMessage("That is not a valid block");
                    return;
                }
            }

            if (args.Length >= 2) {
                try {
                    size = int.Parse(args[1]);
                }
                catch {
                    p.SendMessage("That is not a number");
                    Help(p);
                    return;
                }
            }

            p.SendMessage("Start brushing!!");
            p.ExtraData.ChangeOrCreate<object, object>("BrushData", new BrushData(brushType, block, size));
            p.OnPlayerBlockChange.Normal += BlockChange;

        }

        void BlockChange(Player sender, BlockChangeEventArgs e) {
            e.Cancel();
            
            var raw = (BrushData)sender.ExtraData.GetIfExist<object, object> ("BrushData");

            if(raw == null){
                sender.SendMessage("An error occurred while trying to brush");
                sender.OnPlayerBlockChange.Normal -= BlockChange;
                return;
            }

            byte block = raw.Block != 255 ? raw.Block : e.Holding;
            Vector3S loc = new Vector3S(e.X, e.Z, e.Y);
            IBrush b = (IBrush)Activator.CreateInstance(raw.BrushType);
            var qq = b.Draw(loc, raw.Block, raw.Size);

            foreach (var fml in qq) 
                sender.SendBlockChange((ushort)fml.x, (ushort)fml.z, (ushort)fml.y, block);

#if DEBUG
            sender.SendMessage(string.Format("Brushed {0} blocks", qq.Count())); 
#endif
        }

        public void Help(MCForge.Entity.Player p) {
            p.SendMessage("/brush <brush type> [optional: size] [optional: block]  - place blocks on the level using a brush");
            p.SendMessage("/brush off - turns off the brush...");
            p.SendMessage("Valid brush types:");
            p.SendMessage("Cube, Sphere, Random");
            //TODO: add more types....
        }

        public void Initialize() {
            Command.AddReference(this, "brush", "p");
        }

        #endregion

    }

    class BrushData {
        public Type BrushType;
        public byte Block;
        public int Size;

        public BrushData(Type brushType, byte block, int size) {
            BrushType = brushType;
            Block = block;
            Size = size;
        }
    }
}
