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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Drawing;
using MCForge.Interface;
using MCForge.Utils;
using MCForge.Utils.Settings;
using MCForge.Interfaces;
using MCForge.Entity;
using MCForge.Interface.Command;

namespace MCForge.Core {
    static class Program
    {
        #region console colors
        static Dictionary<Color, ConsoleColor> ConsoleColorMaps = new Dictionary<Color, ConsoleColor>()
    {
        {Color.Black, ConsoleColor.Black},
        {Color.Blue, ConsoleColor.Blue},
        {Color.Cyan, ConsoleColor.Cyan},
        {Color.DarkBlue, ConsoleColor.DarkBlue},
        {Color.DarkCyan, ConsoleColor.DarkCyan},
        {Color.DarkGray, ConsoleColor.DarkGray},
        {Color.DarkGreen, ConsoleColor.DarkGreen},
        {Color.DarkMagenta, ConsoleColor.DarkMagenta},
        {Color.DarkRed, ConsoleColor.DarkRed},
        {Color.FromArgb(204, 119, 34), ConsoleColor.DarkYellow},
        {Color.Gray, ConsoleColor.Gray},
        {Color.Green, ConsoleColor.Green},
        {Color.Magenta, ConsoleColor.Magenta},
        {Color.Red, ConsoleColor.Red},
        {Color.White, ConsoleColor.White},
        {Color.Yellow, ConsoleColor.Yellow}
    };

        static ConsoleColor FindNearestConsoleColor(Color color)
        {
            ConsoleColor bestMatch = ConsoleColor.White;
            int bestVariance = int.MaxValue;

            foreach (Color c in ConsoleColorMaps.Keys)
            {
                int thisVariance = Math.Abs(c.ToArgb() - color.ToArgb());
                if (bestVariance > thisVariance)
                {
                    bestMatch = ConsoleColorMaps[c];
                    bestVariance = thisVariance;
                }
            }

            return bestMatch;
        }
        #endregion
        /// <summary>
        /// Handles the OnRecieveErrorLog event of the Logger control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MCForge.Utils.LogEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private static void Logger_OnRecieveErrorLog(object sender, LogEventArgs e)
        {
            ClearCurrentConsoleLine();
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine("[Error] " + e.Message);
            Console.ForegroundColor = prevColor;
        }

        /// <summary>
        /// Handles the OnRecieveLog event of the Logger control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="message">The <see cref="MCForge.Utils.LogEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private static void Logger_OnRecieveLog(object sender, LogEventArgs message)
        {
            ClearCurrentConsoleLine();
            WriteLine(message.Message, FindNearestConsoleColor(message.TextColor), FindNearestConsoleColor(message.BackgroundColor));
            WriteInputLine("> ", input);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }
        static string input = "";

        static void Main(string[] args)
        {
            ServerSettings.Init();
            cp = new ConsolePlayer(cio);
            bool checker = CheckArgs(args);
            Console.Title = ServerSettings.GetSetting("ServerName") + " - MCForge 6"; //Don't know what MCForge version we are using yet.
            if (!checker)
                new Thread(new ThreadStart(Server.Init)).Start();
            else
                Logger.Log("Aborting Setup..", LogType.Critical);

            //declare the Hooks
            //Error Logging
            Logger.OnRecieveErrorLog += new EventHandler<LogEventArgs>(Logger_OnRecieveErrorLog);
            //Normal Logs
            Logger.OnRecieveLog += new EventHandler<LogEventArgs>(Logger_OnRecieveLog);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ConsoleKeyInfo keyInfo;
            if (Console.CursorLeft < 3)
                WriteInputLine();
            while ((keyInfo = Console.ReadKey()) != null)
            {
                char key = keyInfo.KeyChar;
                //handles escape
                if (keyInfo.Key == ConsoleKey.Escape) continue;
                // Ignore if Alt or Ctrl is pressed
                if ((keyInfo.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
                    continue;
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                    continue;
                // Ignore if KeyChar value is \u0000.
                if (keyInfo.KeyChar == '\u0000') continue;
                // Ignore tab key.
                if (keyInfo.Key == ConsoleKey.Tab) continue;
                // handle function keys
                if (keyInfo.Key >= ConsoleKey.F1 && keyInfo.Key <= ConsoleKey.F24) continue;

                if (keyInfo.Key != ConsoleKey.Enter)
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        ClearConsoleLine(Console.CursorTop);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        if (!(input.Length < 1))
                        {
                            input = input.Remove(input.Length - 1, 1);
                            WriteInputLine("> " + input);
                        }
                    }
                    else if (Char.IsLetterOrDigit(key) || Char.IsPunctuation(key) || Char.IsSymbol(key) || key == ' ')
                        input += key.ToString();
                    continue;
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(input))
                    {
                        WriteInputLine();
                        continue;
                    }
                    WriteLine("");
                    Handle(input);
                    input = "";
                    WriteInputLine();
                }
            }
        }

        private static void Handle(string input)
        {
            //check if it is a command
            if (input.StartsWith("/"))
            {
                ICommand cmd = null;

                string[] commandSplit = input.Remove(0, 1).Split(' ');
                string[] args = commandSplit.Where((val, index) => index != 0).ToArray();
                cmd = Command.Find(commandSplit[0]);

                if (cmd == null)
                {
                    WriteLine("Command not found!");
                    return; // cannot run the command
                }

                cmd.Use(cp, args);
                Logger.Log("CONSOLE used: /" + commandSplit[0]);
            }
            else
            {
                Player.UniversalChat(Colors.white + "[Console] " + Server.DefaultColor + input);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                Logger.Log("[Console] " + input, Color.Yellow, Color.Black, LogType.Normal);
            }
        }
        #region Console Functions
        private static void ClearConsoleLine(int line)
        {
            string l = "";
            for (int i = 0; i <= Console.BufferWidth; i++)
                l += " ";

            int[] oldpos = { Console.CursorLeft, Console.CursorTop };
            Console.SetCursorPosition(0, line);
            Console.Write(l);
            Console.SetCursorPosition(oldpos[0], oldpos[1]);
        }
        private static void ClearCurrentConsoleLine()
        {
            string l = "";
            for (int i = 0; i <= Console.BufferWidth; i++)
                l += " ";

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(l);
            Console.SetCursorPosition(0, Console.CursorTop -1);
        }

        static void Write(string text)
        {
            ClearConsoleLine(Console.CursorTop + 1);
            Console.Write(text);
        }
        public static void WriteLine(string s)
        {
            ClearConsoleLine(Console.CursorTop + 1);
            Console.WriteLine(s);
        }
        public static void WriteLine(string s, ConsoleColor fg, ConsoleColor bg)
        {
            ConsoleColor[] old = { Console.ForegroundColor, Console.BackgroundColor };
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            ClearConsoleLine(Console.CursorTop);
            Console.Write(s);
            Console.ForegroundColor = old[0];
            Console.BackgroundColor = old[1];
            Console.WriteLine();
        }
        public static void Write(string s, ConsoleColor fg, ConsoleColor bg)
        {
            ConsoleColor[] old = { Console.ForegroundColor, Console.BackgroundColor };
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(s);
            Console.ForegroundColor = old[0];
            Console.BackgroundColor = old[1];
        }
        static void WriteInputLine(string inputline = "> ", string input = "")
        {
            if (Console.CursorLeft > 0)
                Console.WriteLine();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(inputline + input);
            Console.SetCursorPosition((inputline + input).Length, Console.CursorTop);
        }
        /// <summary>
        /// Check the args that were passed on program startup
        /// </summary>
        /// <param name="args">The args</param>
        /// <returns>If returns false, run normal setup. If returns true, cancel normal setup. In this case something has already started the server.</returns>
        static bool CheckArgs(string[] args)
        {
            if (args.Length == 0)
                return false;
            string name = args[0];
            switch (name)
            {
                case "load-plugin":
                    if (args.Length == 1)
                        return false;
                    string plugin = args[1];
                    string[] pargs = new string[] { "-force" };
                    for (int i = 1; i < args.Length; i++)
                        pargs[i] = args[i];
                    LoadAllDlls.LoadDLL(plugin, pargs);
                    break;
                case "debug":
                    Server.DebugMode = true;
                    return false;
                case "abort-setup":
                    return true;
            }
            return args[args.Length - 1] == "abort-setup";
        }
        #endregion

        private static ConsolePlayer cp;
        private static CommandIO cio = new CommandIO();
        class CommandIO : IIOProvider
        {
            public string ReadLine()
            {
                if (Console.CursorLeft != 0) Console.WriteLine();
                Console.Write("CIO input: ");
                return Console.ReadLine();
            }

            public void WriteLine(string line)
            {
                if (Console.CursorLeft != 0) Console.WriteLine();
                Console.WriteLine("CIO output: " + line);
            }

            public void WriteLine(string line, string replyChannel)
            {

            }
        }
    }
}
