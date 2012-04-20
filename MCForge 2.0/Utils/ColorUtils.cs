using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MCForge.Utils {
    public static class ColorUtils {
        /// <summary>
        /// A color representing hex value CC 77 22
        /// </summary>
        public static Color DarkYellow {
            get {
                return Color.FromArgb(0xCC7722); //From wikipedia and ConsoleColor metadata
            }
        }

        private static Dictionary<Color, ConsoleColor> colorToConsole;
        private static Dictionary<ConsoleColor, Color> consoleToColor;

        public static void Init() {
            colorToConsole = new Dictionary<Color, ConsoleColor>();

            colorToConsole.Add(Color.Blue, ConsoleColor.Blue);
            colorToConsole.Add(Color.Black, ConsoleColor.Black);
            colorToConsole.Add(Color.Cyan, ConsoleColor.Cyan);
            colorToConsole.Add(Color.DarkBlue, ConsoleColor.DarkBlue);
            colorToConsole.Add(Color.DarkCyan, ConsoleColor.DarkCyan);
            colorToConsole.Add(Color.DarkGray, ConsoleColor.DarkGray);
            colorToConsole.Add(Color.DarkGreen, ConsoleColor.DarkGreen);
            colorToConsole.Add(Color.DarkMagenta, ConsoleColor.DarkMagenta);
            colorToConsole.Add(Color.DarkRed, ConsoleColor.DarkRed);
            colorToConsole.Add(DarkYellow, ConsoleColor.DarkYellow);
            colorToConsole.Add(Color.Gray, ConsoleColor.Gray);
            colorToConsole.Add(Color.Green, ConsoleColor.Green);
            colorToConsole.Add(Color.Magenta, ConsoleColor.Magenta);
            colorToConsole.Add(Color.Red, ConsoleColor.Red);
            colorToConsole.Add(Color.White, ConsoleColor.White);
            colorToConsole.Add(Color.Yellow, ConsoleColor.Yellow);

            consoleToColor = new Dictionary<ConsoleColor, Color>();

            consoleToColor.Add(ConsoleColor.Blue, Color.Blue);
            consoleToColor.Add(ConsoleColor.Black, Color.Black);
            consoleToColor.Add(ConsoleColor.Cyan, Color.Cyan);
            consoleToColor.Add(ConsoleColor.DarkBlue, Color.DarkBlue);
            consoleToColor.Add(ConsoleColor.DarkCyan, Color.DarkCyan);
            consoleToColor.Add(ConsoleColor.DarkGray, Color.DarkGreen);
            consoleToColor.Add(ConsoleColor.DarkGreen, Color.DarkGreen);
            consoleToColor.Add(ConsoleColor.DarkMagenta, Color.DarkMagenta);
            consoleToColor.Add(ConsoleColor.DarkRed, Color.DarkRed);
            consoleToColor.Add(ConsoleColor.DarkYellow, DarkYellow);
            consoleToColor.Add(ConsoleColor.Gray, Color.Gray);
            consoleToColor.Add(ConsoleColor.Green, Color.Green);
            consoleToColor.Add(ConsoleColor.Magenta, Color.Magenta);
            consoleToColor.Add(ConsoleColor.Red, Color.Red);
            consoleToColor.Add(ConsoleColor.White, Color.White);
            consoleToColor.Add(ConsoleColor.Yellow, Color.Yellow);
        }

        /// <summary>
        /// Convert a console color to an ARGB color
        /// </summary>
        /// <param name="color">Console color to convert</param>
        /// <returns>A color</returns>
        public static Color ToColor(ConsoleColor color) {
            return consoleToColor[color];
        }

        /// <summary>
        /// Convert an ARGB color to a Win32 ConsoleColor
        /// </summary>
        /// <param name="color">Color to convert</param>
        /// <remarks>If no ConsoleColor is found to convert, ConsoleColor.Black will be returned</remarks>
        /// <returns>A converted ConsoleColor</returns>
        public static ConsoleColor ToConsoleColor(Color color) {
            if (colorToConsole.ContainsKey(color))
                return colorToConsole[color];
            return ConsoleColor.Black;
        }
    }
}