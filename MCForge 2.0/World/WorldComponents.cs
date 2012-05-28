using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Core;
using MCForge.Utils;
using MCForge.Entity;

namespace MCForge.World
{
    public class WorldComponent
    {

        private BlockComponent[] components;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldComponent"/> class.
        /// </summary>
        /// <param name="xyz">The pieces of the component.</param>
        public WorldComponent(params BlockComponent[] components)
        {
            this.components = components;
        }

        /// <summary>
        /// Draws the on level.
        /// </summary>
        /// <param name="mLevel">The level to draw on.</param>
        /// <param name="startPos">The start pos.</param>
        /// <param name="blockOverlay">A block to set the component as. If it is not already set</param>
        /// <remarks>if the block in the component is set to 255 a block overlay will be used automaticly, no block change will occur if the block is at 254</remarks>
        public void DrawOnLevel(Level mLevel, Vector3S startPos, byte blockOverlay = 1)
        {
            for (int i = 0; i < components.Length; i++)
            {
                var comp = components[i];
                if (comp.Block == 254)
                    continue;
                mLevel.SetBlock(comp.Pos + startPos, comp.Block == 255 ? blockOverlay : comp.Block);
            }
        }
        private static short IncDec(string direction, short value, short by)
        {
            if (direction == "r" || direction == "u") { return (short)(value + by); }
            if (direction == "l" || direction == "d") { return (short)(value - by); }
            return -1;
        }
        public static void DrawString(Player p, short _x, short _z, short _y, string ToWrite, string dir, Block Holding)
        {
            short x = _x, z = _z, y = _y, sx = _x, sz = _z, sy = _y;
            foreach (char c in ToWrite)
            {
                switch (char.ToUpper(c)) //TODO: add all letters support (including lowercase)
                {
                    case 'A':
                        for (short yy = 0; yy <= 5; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (short yy = 0; yy <= 5; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'B':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (double d = 1; d <= 5; d++) { if (d == 4) { continue; } p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + d)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'C':
                        for (short yy = 1; yy <= 5; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 1)), Holding);
                        p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 5)), Holding);
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'D':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 1; i <= 5; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'E':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 4; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            if (i <= 2) { p.Level.BlockChange(new Vector3D((double)x, (double)(z), (double)(y + 4)), Holding); }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'F':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 4; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            if (i <= 2) { p.Level.BlockChange(new Vector3D((double)x, (double)(z), (double)(y + 4)), Holding); }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'G':
                        for (short yy = 1; yy <= 5; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++)
                        {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 1; i <= 6; i++)
                        {
                            if (i == 5) { continue; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding);
                        }
                        if (dir == "r") { z--; } if (dir == "l") { z++; } if (dir == "u") { x--; } if (dir == "d") { x++; }
                        p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                        if (dir == "r") { z += 3; } if (dir == "l") { z -= 3; } if (dir == "u") { x += 3; } if (dir == "d") { x -= 3; }
                        break;
                    case 'H':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'I':
                        for (short zz = 1; zz <= 3; zz++) {
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                            if (zz < 3) { if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; } }
                        }
                        if (dir == "r") { z--; } if (dir == "l") { z++; } if (dir == "u") { x--; } if (dir == "d") { x++; }
                        for (int i = 1; i <= 5; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding); }
                        if (dir == "r") { z += 3; } if (dir == "l") { z -= 3; } if (dir == "u") { x += 3; } if (dir == "d") { x -= 3; }
                        break;
                    case 'J':
                        p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 1)), Holding);
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 1; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'K':
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        for (int i = 1; i <= 2; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 0; i <= 5; i++) { if (i != 3 && i != 5) { continue; } p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding); }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 0; i <= 6; i++) { if (i == 3 || i == 4 || i == 5) { continue; } p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'L':
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        for (int i = 1; i <= 4; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                        }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'M':
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, i == 2 ? (double)(y + 4) : (double)(y + 5)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'N':
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        for (int i = 5; i >= 3; i--) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'O':
                        for (short yy = 1; yy <= 5; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (short yy = 1; yy <= 5; yy++)  { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'P':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 5)), Holding);
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'Q':
                        for (short yy = 1; yy <= 5; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            if (i < 3) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding); }
                            else { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 1)), Holding); }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (short yy = 0; yy <= 5; yy++) { if (yy == 1) { continue; } p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'R':
                        for (short yy = 0; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (double d = 0; d <= 5; d++) { if (d == 4) { continue; } p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + d)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'S':
                        for (int i = 0; i <= 3; i++) {
                            if (i == 0) {
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 1)), Holding);
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 5)), Holding);
                            }
                            else {
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 4)), Holding);
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                            }
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        }
                        for (int i = 1; i <= 6; i++) { if (i == 4 || i == 5) { continue; } p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'T':
                        for (int i = 1; i <= 5; i++) {
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6)), Holding);
                            if (i == 3) { for (int j = 6; j >= 0; j--) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + 6 - j)), Holding); } }
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        break;
                    case 'U':
                        for (short yy = 1; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)y), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (short yy = 1; yy <= 6; yy++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + yy)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'V':
                        for (int i = 0; i <= 5; i++)
                        {
                            if (i == 4) { if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; } }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + 6 - i)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y)), Holding);
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 1; i <= 6; i++) {
                            if (i == 3) { if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; } }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); 
                        }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'W':
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        for (int i = 1; i <= 3; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, i == 2 ? (double)(y + 2) : (double)(y + 1)), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 0; i <= 6; i++) { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'X':
                        for (int i = 0; i <= 6; i++) {
                            if (i == 3 || i == 4) { if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; } }
                            if (i == 5 || i == 6) { if (dir == "r") { z--; } if (dir == "l") { z++; } if (dir == "u") { x--; } if (dir == "d") { x++; } }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding);
                        }
                        if (dir == "r") { z += 4; } if (dir == "l") { z -= 4; } if (dir == "u") { x += 4; } if (dir == "d") { x -= 4; }
                        for (int i = 0; i <= 6; i++) {
                            if (i == 3) { if (dir == "r") { z--; } if (dir == "l") { z++; } if (dir == "u") { x--; } if (dir == "d") { x++; } }
                            if (i == 4) { continue; }
                            if (i == 6) { if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; } }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding);
                        }
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        break;
                    case 'Y':
                        if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                        for (int i = 0; i <= 6; i++) {
                            if (i == 5) {
                                if (dir == "r") { z--; } if (dir == "l") { z++; } if (dir == "u") { x--; } if (dir == "d") { x++; }
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding);
                                if (dir == "r") { z += 2; } if (dir == "l") { z -= 2; } if (dir == "u") { x += 2; } if (dir == "d") { x -= 2; }
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding);
                            }
                            if (i == 6) {
                                if (dir == "r") { z -= 3; } if (dir == "l") { z += 3; } if (dir == "u") { x -= 3; } if (dir == "d") { x += 3; }
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding);
                                if (dir == "r") { z += 4; } if (dir == "l") { z -= 4; } if (dir == "u") { x += 4; } if (dir == "d") { x -= 4; }
                                p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding);
                            }
                            else { p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(sy + i)), Holding); }
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        break;
                    case 'Z':
                        for (int i = 0; i <= 4; i++) {
                            if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y)), Holding);
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)((y + 6))), Holding);
                        }
                        if (dir == "r") { z++; } if (dir == "l") { z--; } if (dir == "u") { x++; } if (dir == "d") { x--; }
                        for (int i = 5; i >= 1; i--) {
                            if (dir == "r") { z--; } if (dir == "l") { z++; } if (dir == "u") { x--; } if (dir == "d") { x++; }
                            p.Level.BlockChange(new Vector3D((double)x, (double)z, (double)(y + i)), Holding);
                        }
                        if (dir == "r") { z += 6; } if (dir == "l") { z -= 6; } if (dir == "u") { x += 6; } if (dir == "d") { x -= 6; }
                        break;
                        
                    case ' ':
                        if (dir == "r") { z += 4; } if (dir == "l") { z -= 4; } if (dir == "u") { x += 4; } if (dir == "d") { x -= 4; }
                        break;
                    default:
                        p.SendMessage("Sorry the letter \"" + c + "\" hasn't been implemented yet!");
                        break;
                }
            }
        }
        /// <summary>
        /// A static component for the letter 'A' (capitalized)
        /// </summary>
        public static readonly WorldComponent LetterA = new WorldComponent(
            new BlockComponent(255, new Vector3S(1, 0, 0)), new BlockComponent(255, new Vector3S(2, 0, 0)),
            new BlockComponent(255, new Vector3S(0, 0, 1)), new BlockComponent(255, new Vector3S(3, 0, 1)),
            new BlockComponent(255, new Vector3S(0, 0, 2)), new BlockComponent(255, new Vector3S(1, 0, 2)), new BlockComponent(255, new Vector3S(2, 0, 2)), new BlockComponent(255, new Vector3S(3, 0, 2)),
            new BlockComponent(255, new Vector3S(0, 0, 3)), new BlockComponent(255, new Vector3S(3, 0, 3)),
            new BlockComponent(255, new Vector3S(0, 0, 4)), new BlockComponent(255, new Vector3S(3, 0, 4)),
            new BlockComponent(255, new Vector3S(0, 0, 5)), new BlockComponent(255, new Vector3S(3, 0, 5))
       );
    }

    /// <summary>
    /// A struct containting a Block and position
    /// </summary>
    public struct BlockComponent
    {
        /// <summary>
        /// The type of block
        /// </summary>
        public byte Block;
        /// <summary>
        /// The position
        /// </summary>
        public Vector3S Pos;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockComponent"/> struct.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="mVec">The position.</param>
        public BlockComponent(byte block, Vector3S mVec)
        {
            this.Block = block;
            this.Pos = mVec;
        }

    }
}