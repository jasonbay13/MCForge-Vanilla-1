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
using System.Linq;
using System.Text;
using System.Threading;
using MCForge;
using System.IO;

namespace CommandDll
{
    public class CmdCalculate : ICommand
    {
        public string Name { get { return "Calculate"; } }
        public CommandTypes Type { get { return CommandTypes.information; } }
        public string Author { get { return "Gamemakergm"; } }
        public int Version { get { return 1; } }
        public string CUD { get { return ""; } }

        public void Use(Player p, string[] args)
        {
            try
            {
                double result = 0;
                float num1 = 0;
                float num2 = 0;
                String operation = args[1];

                if (args.Length == 2)
                {
                    if (!float.TryParse(args[0], out num1))
                    {
                        p.SendMessage("An invalid number was given!");
                        return;
                    }
                    switch (operation)
                    {
                        case "square":
                            result = Math.Pow(num1, 2);
                            goto Meep;
                        case "root":
                            result = Math.Sqrt(num1);
                            goto Meep;
                        case "cube":
                            result = Math.Pow(num1, 3);
                            goto Meep;
                        case "pi":
                            result = num1 * Math.PI;
                            goto Meep;
                        default:
                            p.SendMessage("There is no such method.");
                            return;
                    }
                Meep:
                    p.SendMessage((operation == "pi") ? ("The answer:&a " + num1 + " x PI " + Colors.yellow + " = " + Colors.red + result) : ("The answer: &aThe " + operation + " of " + num1 + Colors.yellow + " = " + Colors.red + result));
                    return;
                }
                else if (args.Length == 3)
                {
                    if (!float.TryParse(args[2], out num2) || !float.TryParse(args[0], out num1))
                    {
                        p.SendMessage("An invalid number was given!");
                        return;
                    }
                    num1 = float.Parse(args[0]);
                    num2 = float.Parse(args[2]);
                    switch (operation)
                    {
                        case "x":
                        case "*":
                            result = num1 * num2;
                            goto _Meep;
                        case "+":
                            result = num1 + num2;
                            goto _Meep;
                        case "-":
                            result = num1 - num2;
                            goto _Meep;
                        case "/":
                            if (num2 == 0)
                            {
                                p.SendMessage("Cannot divide by 0!");
                                return;
                            }
                            result = num1 / num2;
                            goto _Meep;
                      case "^":
                            result = Math.Pow(num1, num2);
                            goto _Meep;
                        default:
                            p.SendMessage("There is no such method.");
                            return;
                    }
                _Meep:
                    p.SendMessage("The answer:&a " + num1 + " " + operation + " " + num2 + Colors.yellow + " = " + Colors.red + result);
                    return;
                }
                else
                    Help(p);
                return;
            }
            //Invalid arguments
            catch (IndexOutOfRangeException)
            {
                Help(p);
                return;
            }
        }

        public void Help(Player p)
        {
            p.SendMessage("/calculate <num1> <method> <num2> - Evaluates two number using a method.");
            p.SendMessage("Available methods for two numbers: /, x, -, +, ^(Exponent),");
            p.SendMessage("/calculate <num1> <method> - Evaluates a number using a complex method.");
            p.SendMessage("Available methods for one number: square, root, pi, cube");
        }

        public void Initialize()
        {
            Command.AddReference(this, new string[2] { "calculate", "calc" });
        }
    }
}
