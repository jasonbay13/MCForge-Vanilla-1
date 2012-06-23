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
using System;
using MCForge.Utils;
using MCForge.World;
using MCForge.Interfaces.Blocks;

namespace MCForge.Commands {
    public class CmdMessageBlock : ICommand {
        public CmdMessageBlock() {
        }
        public string Name {
            get { return "MessageBlock"; }
        }

        public CommandTypes Type {
            get { return CommandTypes.Misc; }
        }

        public string Author {
            get { return "ninedrafted"; }
        }

        public int Version {
            get { return 1; }
        }

        public string CUD {
            get { return ""; }
        }

        public byte Permission {
            get { return 0; }
        }

        public void Use(Player p, string[] args) {
            p.ExtraData[Name + p.Username] = String.Join(" ", args);
            p.OnPlayerBlockChange.Normal += new API.Events.Event<Player, API.Events.BlockChangeEventArgs>.EventHandler(OnPlayerBlockChange_Normal);
            p.SendMessage("Place one block to define the location");
        }

        void OnPlayerBlockChange_Normal(Player sender, API.Events.BlockChangeEventArgs args) {
            sender.OnPlayerBlockChange.Normal -= OnPlayerBlockChange_Normal;
            string message = (string)sender.ExtraData[Name + sender.Username];
            Vector3S blockPos = new Vector3S(args.X, args.Z, args.Y);
            IBlock block = MCForge.Interfaces.Blocks.Block.GetBlock("MessageBlock");
            if (block != null && block.Name == "MessageBlock") {
                ((MessageBlock)block).AddMessage(blockPos, message, args.Holding, sender.Level);
            }
            else {
                sender.SendMessage("There is no MessageBlock loaded");
            }
            args.Cancel();
        }

        public void Help(Player p) {
            p.SendMessage("/messageblock [message]");
            p.SendMessage("Creates a message block.");
            p.SendMessage("Shortcut: /mb");
        }

        public void Initialize() {
            Command.AddReference(this, new string[2] { "MessageBlock", "mb" });
        }
    }

    public class MessageBlock : IBlock {

        public string Name {
            get { return "MessageBlock"; }
        }

        public byte GetDisplayType(Vector3S blockPosition, Level level) {
            byte ret = 0;
            try {
                ret = byte.Parse((string)level.ExtraData[Name + blockPosition + "block"]);
            }
            catch { }
            return ret;
        }

        public void OnPlayerStepsOn(Player p, Vector3S blockPosition, Level level) {
            string text = (string)level.ExtraData[Name + blockPosition + "message"];
            if (text == null) p.SendMessage("No message stored");
            else p.SendMessage(text);
        }

        public bool OnAction(Player p, Vector3S blockPosition, byte block, Level level) {
            string text = (string)level.ExtraData[Name + blockPosition + "message"];
            if (block == 0) {
                ICommand c = Command.Find("messageblock");
                if (c != null && p.Group.Permission >= c.Permission) {
                    p.SendMessage("Removing message block: " + text);
                    return false;
                }
            }
            if (text == null) p.SendMessage("No message stored");
            else p.SendMessage(text);
            return true;
        }

        public void Initialize() {
            MCForge.Interfaces.Blocks.Block.AddReference(this);
        }

        public void OnUnload() {

        }
        public void AddMessage(Vector3S blockPosition, string text, byte holding, Level level) {
            level.ExtraData[Name + blockPosition + "block"] = "" + holding;
            AddMessage(blockPosition, text, level);
        }
        public void AddMessage(Vector3S blockPosition, string text, Level level) {
            level.ExtraData[Name + blockPosition + "message"] = text;
            MCForge.Interfaces.Blocks.Block.SetBlock(this, blockPosition, level);
        }
        public void OnRemove(Vector3S blockPosition, Level level) {
            level.ExtraData.Remove(Name + blockPosition + "block");
            level.ExtraData.Remove(Name + blockPosition + "message");
        }

        public void PhysicsTick(Vector3S[] blockPositions, Level level) {
        }
    }
}