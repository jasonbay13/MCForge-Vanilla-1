using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interfaces.Blocks;
using MCForge.Utils;

namespace MCForge.Commands {
    public class BlockDoor : IBlock {
        protected string _name = "BlockDoor";
        public string Name {
            get { return _name; }
        }
        protected byte ClosedBlock = 1;
        protected byte OpenBlock = 3;
        public byte GetDisplayType(Utils.Vector3S blockPosition, World.Level level) {
            if (level.ExtraData[Name + blockPosition + "open"] == null || (string)level.ExtraData[Name + blockPosition + "open"] == "false") {
                return ClosedBlock;
            }
            else return OpenBlock;
        }

        public void OnPlayerStepsOn(Entity.Player p, Utils.Vector3S blockPosition, World.Level level) {
            p.SendMessage("Step on: " + blockPosition);
        }

        public bool OnAction(Entity.Player p, Utils.Vector3S blockPosition, byte holding, World.Level level) {
            level.ExtraData[Name + blockPosition + "open"] = blockPosition.ToString();
            level.ExtraData[Name + blockPosition + "opener"] = blockPosition.ToString();
            level.ExtraData[Name + blockPosition + "tick"] = 0;
            return true;
        }
        public int TicksOpen = 15;
        public void PhysicsTick(Utils.Vector3S[] blockPositions, World.Level level) {
        /*    List<Vector3S> ticked = new List<Vector3S>();
            foreach (Vector3S blockPos in blockPositions) {
                if (ticked.Contains(blockPos, blockPos)) continue;
                ticked.Add(blockPos);
                if (level.ExtraData[Name + blockPos + "tick"] != null && level.ExtraData[Name + blockPos + "tick"].GetType() == typeof(string)) {
                    level.ExtraData[Name + blockPos + "tick"] = int.Parse((string)level.ExtraData[Name + blockPos + "tick"]);
                }
                if (level.ExtraData[Name + blockPos + "tick"] != null) {
                    if ((int)level.ExtraData[Name + blockPos + "tick"] == 0) {
                        Console.WriteLine(blockPos);
                        level.ExtraData[Name + blockPos + "open"] = "true";
                        Block.SendBlock(blockPos, level);
                        level.ExtraData[Name + blockPos + "tick"] = (int)level.ExtraData[Name + blockPos + "tick"] + 1;
                        foreach (Tuple<string, Vector3S> sv in Block.GetNeighborsNames(blockPos, level)) {
                            if (sv.Item1 == Name) {
                                Console.WriteLine("! " + sv.Item2);
                                if ((string)level.ExtraData[Name + blockPos + "opener"] != (string)level.ExtraData[Name + sv.Item2 + "opener"]) {
                                    level.ExtraData[Name + sv.Item2 + "opener"] = (string)level.ExtraData[Name + blockPos + "opener"];
                                    level.ExtraData[Name + sv.Item2 + "tick"] = 0;
                                    ticked.Add(sv.Item2);
                                }
                            }
                        }
                    }
                    else if ((int)level.ExtraData[Name + blockPos + "tick"] == TicksOpen) {
                        level.ExtraData[Name + blockPos + "tick"] = null;
                        level.ExtraData[Name + blockPos + "open"] = "false";
                        level.ExtraData[Name + blockPos + "opener"] = "false";
                        Block.SendBlock(blockPos, level);
                    }
                    else {
                        level.ExtraData[Name + blockPos + "tick"] = ((int)level.ExtraData[Name + blockPos + "tick"]) + 1;
                    }
                }
            }*/
        }


        public void Initialize() {
            Block.AddReference(this);
        }

        public void OnUnload() {
        }

        public void OnRemove(Vector3S blockPosition, World.Level level) {
            level.ExtraData[Name + blockPosition + "tick"] = null;
            level.ExtraData[Name + blockPosition + "open"] = null;
            level.ExtraData[Name + blockPosition + "opener"] = null;
        }
    }
}
