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
using System.Collections.Generic;
using System.Threading;
using MCForge;
using MCForge.Interface.Command;
using MCForge.Entity;
using MCForge.Core;
using MCForge.World;
using MCForge.World.Blocks;

namespace CommandDll
{
    public class CmdFly : ICommand
    {
        public string Name { get { return "Fly"; } }
        public CommandTypes Type { get { return CommandTypes.Misc; } }
        public string Author { get { return "Gamemakergm"; } }
        public decimal Version { get { return 1.00m; } }
        public string CUD { get { return ""; } }
        public byte Permission { get { return 30; } }

        public void Use(Player p, string[] args)
        {
            p.isFlying = !p.isFlying;
            if (!p.isFlying)
            {
                return;
            }
            p.SendMessage("You are now flying. &cJump!");

            Thread fly = new Thread(new ThreadStart(delegate
                {
                Vector3 pos;
                Vector3 oldpos = new Vector3();
                List<Vector3> buffer = new List<Vector3>();
                while (p.isFlying)
                {
                    Thread.Sleep(20);
                    if (p.Pos.x == oldpos.x && p.Pos.z == oldpos.z && p.Pos.y == oldpos.y) continue;
                    try
                    {
                        List<Vector3> tempBuffer = new List<Vector3>();
                        List<Vector3> toRemove = new List<Vector3>();
                        ushort x = (ushort)((p.Pos.x) / 32);
                        ushort z = (ushort)((p.Pos.z) / 32);
                        ushort y = (ushort)((p.Pos.y - 60) / 32);
                        try
                        {
                            for (ushort xx = (ushort)(x - 1); xx <= x + 1; xx++)
                            {
                                for (ushort yy = (ushort)(y - 1); yy <= y; yy++)
                                {
                                    for (ushort zz = (ushort)(z - 1); zz <= z + 1; zz++)
                                    {
                                        if (p.Level.GetBlock(xx, zz, yy) == new Air().VisibleBlock)
                                        {
                                            pos.x = (short)xx; pos.y = (short)yy; pos.z = (short)zz;
                                            tempBuffer.Add(pos);
                                        }
                                    }
                                }
                            }
                            foreach (Vector3 cP in tempBuffer)
                            {
                                if (!buffer.Contains(cP))
                                {
                                    buffer.Add(cP);
                                    p.SendBlockChange((ushort)cP.x, (ushort)cP.z, (ushort)cP.y, new Glass().VisibleBlock);
                                }
                            }
                            foreach (Vector3 cP in buffer)
                            {
                                if (!tempBuffer.Contains(cP))
                                {
                                    p.SendBlockChange((ushort)cP.x, (ushort)cP.z, (ushort)cP.y, new Air().VisibleBlock);
                                    toRemove.Add(cP);
                                }
                            }
                            foreach (Vector3 cP in toRemove)
                            {
                                buffer.Remove(cP);
                            }
                            tempBuffer.Clear();
                            toRemove.Clear();
                        }
                        catch { }
                    }
                    catch { }
                    //
                    //p.Pos.CopyTo(oldpos, 0);
                }

                foreach (Vector3 cP in buffer)
                {
                    p.SendBlockChange((ushort)cP.x, (ushort)cP.z, (ushort)cP.y, new Air().VisibleBlock);
                }

                p.SendMessage("Stopped flying");
            }));
            fly.Start();
           
        }
        public void Help(Player p)
        {
            p.SendMessage("/fly - Allows you to fly");
        }

        public void Initialize()
        {
            Command.AddReference(this, "fly");
        }
    }
}
