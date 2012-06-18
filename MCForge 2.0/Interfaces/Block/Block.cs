using System.Collections.Generic;
using System.IO;
using System;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.Interface;
using MCForge.Entity;
using MCForge.Utils;
using MCForge.World;
using MCForge.API.Events;

namespace MCForge.Interfaces.Blocks {
    public class Block {
        static Block() {
            Player.OnAllPlayersMove.SystemLvl += new Event<Player, MoveEventArgs>.EventHandler(OnAllPlayersMove_SystemLvl);
        }

        static void OnAllPlayersMove_SystemLvl(Player sender, MoveEventArgs args) {
            IBlock b = GetBlock(sender.belowBlock, sender.Level);
            if (b != null) {
                Vector3S old = new Vector3S(sender.oldPos);
                old.y -= 64;
                old = old / 32;
                if (old != sender.belowBlock)
                    b.OnPlayerStepsOn(sender, sender.belowBlock, sender.Level);
            }
        }
        internal static ExtraData<string, IBlock> Blocks = new ExtraData<string, IBlock>();
        /// <summary>
        /// Get the names of all plugins
        /// </summary>
        /// <returns>The names of all plugins</returns>
        public static string[] GetNames() {
            List<string> ret = new List<string>();
            foreach (string name in Blocks.Keys) {
                ret.Add(name);
            }
            return ret.ToArray();
        }
        /// <summary>
        /// Unloads a block type
        /// </summary>
        /// <param name="name">The name of the block type to unload</param>
        /// <param name="ignoreCase">Wheter or not to ignore the case. (default true)</param>
        /// <returns>Wheter or not the block is unloaded</returns>
        public static bool unload(string name, bool ignoreCase = true) {
            foreach (string key in Blocks.Keys) {
                if ((ignoreCase && Blocks[key].Name.ToLower() == name.ToLower()) || Blocks[key].Name == name) {

                    try {
                        Blocks[key].OnUnload();
                    }
                    catch { Logger.Log("[Block] " + Blocks[key].Name + " cannot be unloaded", LogType.Warning); }
                    Blocks.Remove(key);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Unloads all blocks
        /// </summary>
        /// <returns>Wheter or not all blocks are unloaded</returns>
        public static bool unloadAll() {
            foreach (string key in Blocks.Keys) {
                try {
                    Blocks[key].OnUnload();
                }
                catch { Logger.Log("[Block] " + key + " cannot be unloaded", LogType.Warning); }
                Blocks.Remove(key);
            }
            return false;
        }

        private static bool isLoaded(string name, bool ignoreCase = true) {
            foreach (string key in Blocks.Keys) {
                if ((ignoreCase && Blocks[key].Name.ToLower() == name.ToLower()) || Blocks[key].Name == name)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Reload one or all unloaded plugins.
        /// </summary>
        /// <param name="name">The name of the plugin to load, or an empty string to load all plugins</param>
        /// <param name="ignoreCase">Whether the case of the name gets ignored or not</param>
        /// <returns></returns>
        public static int reload(string name = "", bool ignoreCase = true) {
            foreach (string key in Blocks.Keys) {
                if (name != "") {
                    if (ignoreCase) {
                        if (Blocks[key].Name.ToLower() == name.ToLower()) {
                            unload(name);
                            break;
                        }
                    }
                    else
                        if (Blocks[key].Name == name) {
                            unload(name);
                            break;
                        }
                }
                else {
                    unload(Blocks[key].Name);
                    Logger.Log("[Block] Unloaded " + key);
                }
            }

            int ret = 0;
            string path = Directory.GetCurrentDirectory();
            string[] DLLFiles = Directory.GetFiles(path, "*.dll");
            foreach (string s in DLLFiles) {
                ret++;
                if (name == "")
                    LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, typeof(IBlock));
                else if (ignoreCase) {
                    if (s.ToLower() == name.ToLower())
                        LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, typeof(IBlock));
                }
                else
                    if (s == name)
                        LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, typeof(IBlock));
            }
            if (ServerSettings.HasKey("BlockPath")) {
                string pluginspath = ServerSettings.GetSetting("BlockPath");
                if (Directory.Exists(pluginspath)) {
                    DLLFiles = Directory.GetFiles(pluginspath, "*.dll");
                    foreach (string s in DLLFiles) {
                        if (name == "")
                            LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, typeof(IBlock));
                        else if (ignoreCase) {
                            if (s.ToLower() == name.ToLower())
                                LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, typeof(IBlock));
                        }
                        else
                            if (s == name)
                                LoadAllDlls.LoadDLL(s, new string[] { "-normal" }, typeof(IBlock));
                        ret++;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Adds a reference to the block (autmatically called)
        /// </summary>
        /// <param name="plugin">The block d that this reference... references, you should most likely use 'this'</param>    
        public static void AddReference(IBlock block) {
            Blocks[block.Name] = block;
            foreach (Level l in Level.Levels) {
                if (l.ExtraData["IBlocks" + block.Name] != null && l.ExtraData["IBlocks" + block.Name].GetType() == typeof(string)) {
                    string hexs = (string)l.ExtraData["IBlocks" + block.Name];
                    l.ExtraData["IBlocks" + block.Name] = new List<string>();
                    ((List<string>)l.ExtraData["IBlocks" + block.Name]).AddHexstrings(hexs);
                }
                else if (l.ExtraData["IBlocks" + block.Name] == null) {
                    l.ExtraData["IBlocks" + block.Name] = new List<string>();
                }
            }
        }
        /// <summary>
        /// Gets a block by interface.
        /// </summary>
        /// <param name="name">The name of the interface</param>
        /// <returns></returns>
        public static IBlock getByInterface(string name) {
            foreach (string key in Blocks.Keys) {
                if (Blocks[key].GetType().GetInterface(name) != null)
                    return Blocks[key];
            }
            return null;
        }
        public static bool DoAction(Player p, ushort x, ushort z, ushort y, byte holding, Level level) {
            Vector3S blockPos = new Utils.Vector3S(x, z, y);
            string name = (string)level.ExtraData["IBlocks" + blockPos.ToString()];
            return DoAction(name, p, blockPos, holding, level);

        }
        public static bool DoAction(string name, Player p, Vector3S blockPosition, byte holding, Level level) {
            if (Blocks[name] != null) {
                return Blocks[name].OnAction(p, blockPosition, holding, level);
            }
            else return false;
        }
        public static void OnPlayerStepsOn(string name, Player p, Vector3S blockPosition, Level level) {
            if (Blocks[name] != null) {
                Blocks[name].OnPlayerStepsOn(p, blockPosition, level);
            }
        }
        public static byte GetVisibleType(ushort x, ushort z, ushort y, Level level) {
            Vector3S blockPos = new Utils.Vector3S(x, z, y);
            string name = (string)level.ExtraData["IBlocks" + blockPos.ToString()];
            return GetVisibleType(name, blockPos, level);
        }
        public static byte GetVisibleType(string name, Vector3S blockPos, Level level) {
            if (Blocks[name] != null) return Blocks[name].GetDisplayType(blockPos, level);
            else return 0;
        }
        public static IBlock GetBlock(Vector3S blockPos, Level level) {
            string name = (string)level.ExtraData["IBlocks" + blockPos.ToString()];
            return Blocks[name];
        }
        public static void SetBlock(IBlock block, Vector3S blockPosition, Level level) {
            level.ExtraData["IBlocks" + blockPosition.ToString()] = block.Name;
            if (level.ExtraData["IBlocks" + block.Name] == null)
                level.ExtraData["IBlocks" + block.Name] = new List<string>();
            ((List<string>)level.ExtraData["IBlocks" + block.Name]).Add(blockPosition.ToString());
            level.SetBlock(blockPosition, 255);
        }
        public static void RemoveBlock(Vector3S blockPosition, Level level) {
            level.SetBlock(blockPosition, 0);
            string name = (string)level.ExtraData["IBlocks" + blockPosition.ToString()];
            if (name != null && Blocks[name] != null) Blocks[name].OnRemove(blockPosition, level);
            level.ExtraData.Remove("IBlocks" + blockPosition.ToString());
            ((List<string>)level.ExtraData["IBlocks" + name]).Remove(blockPosition.ToString());
        }
        public static IBlock GetBlock(string name) {
            return Blocks[name];
        }
        public static IEnumerable<Tuple<string,Vector3S>> GetNeighborsNames(Vector3S blockPos, Level level) {
            foreach (Vector3S v in blockPos.GetNeighbors()) {
                string name =(string)level.ExtraData["IBlocks" + blockPos];
                if (name != null) yield return new Tuple<string, Vector3S>(name, v);
            }
        }
        public static Vector3S[] GetPositions(string name, Level level) {
            if (level.ExtraData["IBlocks" + name] != null && level.ExtraData["IBlocks" + name].GetType() == typeof(string)) {
                string hexs = (string)level.ExtraData["IBlocks" + name];
                level.ExtraData["IBlocks" + name] = new List<string>();
                ((List<string>)level.ExtraData["IBlocks" + name]).AddHexstrings(hexs);
            }
            List<string> pos = ((List<string>)level.ExtraData["IBlocks" + name]);
            if (pos != null) {
                Vector3S[] ret = new Vector3S[pos.Count];
                for (int i = 0; i < ret.Length; i++) {
                    ret[i] = new Utils.Vector3S();
                    ret[i].FromString(pos[i]);
                }
                return ret;
            }
            return new Vector3S[0];
        }
        public static void DoTick(Level level) {
            foreach (string name in Blocks.Keys) {
                Blocks[name].PhysicsTick(GetPositions(name, level), level);
            }
        }
        public static void SendBlock(Vector3S blockPosition, Level l) {
            Player.GlobalBlockchange(l, (ushort)blockPosition.x, (ushort)blockPosition.z, (ushort)blockPosition.y, GetBlock(blockPosition, l).GetDisplayType(blockPosition, l));
        }
    }
}
