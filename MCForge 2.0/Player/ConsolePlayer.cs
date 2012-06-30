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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Interfaces;
using MCForge.API.Events;
using System.Reflection;

namespace MCForge.Entity {
    public class ConsolePlayer : Player {
        public ConsolePlayer(IIOProvider io) {
            this.IO = io;
            base.OnCommandEnd.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerBlockChange.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerChat.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerCommand.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerConnect.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerDisconnect.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerMove.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerReceivePacket.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerReceiveUnknownPacket.OnRegister += new EventHandler<EventRegisterArgs>(OnRegister);

            base.OnCommandEnd.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerBlockChange.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerChat.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerCommand.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerConnect.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerDisconnect.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerMove.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerReceivePacket.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);
            base.OnPlayerReceiveUnknownPacket.OnUnregister += new EventHandler<EventRegisterArgs>(OnRegister);

        }
        void OnRegister(object sender, EventRegisterArgs args) {
            if (args.Registering) {
                IO.WriteLine(args.Method.Target.GetType().Name + " registered to receive " + args.ArgsType.Name + " from " + args.SenderType.Name);
                AskForEventArgs(args);
            }
            else {
                IO.WriteLine(args.Method.Target.GetType().Name + " aborted to receive " + args.ArgsType.Name + " from " + args.SenderType.Name);
            }
        }
        void AskForEventArgs(EventRegisterArgs args) {
            if (typeof(Player) == args.SenderType) {
                object oargs = createInstance(selectConstructer(args.ArgsType));
                args.Method.Method.Invoke(args.Method.Target, new object[] {this, oargs });
            }
        }
        ConstructorInfo selectConstructer(Type t) {
            ConstructorInfo[] ci = t.GetConstructors();
            for (int i = 0; i < ci.Length; i++) {
                IO.WriteLine(i + ": " + ci[i]);
            }
            if (ci.Length == 1) return ci[0];
            IO.WriteLine("Select a constructer for " + t.Name);
            string read = IO.ReadLine();
            int c = 0;
            try {
                c = Int32.Parse(read);
            }
            catch { return null; }
            if (c < ci.Length && c >= 0)
                return ci[c];
            return null;
        }
        object createInstance(ConstructorInfo c) {
            ParameterInfo[] pi=c.GetParameters();
            object[] para = new object[pi.Length];
            bool singleLine = true;
            for (int i = 0; i < pi.Length; i++) {
                if (!pi[i].ParameterType.IsValueType) singleLine = false;
            }
            if (singleLine) {
                string types = "";
                for (int i = 0; i < pi.Length; i++) {
                    types += "[ " + pi[i].Name + " as " + pi[i].ParameterType + " ]";
                }
                IO.WriteLine("Enter all values splitted by a whitespace\n" + types);
                string val = IO.ReadLine();
                string[] vals = val.Split(' ');
                if (vals.Length >= para.Length) {
                    for (int i = 0; i < para.Length; i++) {
                        para[i] = createValueType(pi[i], vals[i]);
                    }
                }
                else singleLine = false;
            }
            if (!singleLine) {
                for (int i = 0; i < pi.Length; i++) {
                    if (pi[i].ParameterType.IsValueType || pi[i].ParameterType == typeof(string)) {
                        IO.WriteLine("Enter value for [ " + pi[i].Name + " as " + pi[i].ParameterType.Name+" ]");
                        string val = IO.ReadLine();
                        para[i] = createValueType(pi[i], val);
                    }
                    else {
                        para[i] = createInstance(selectConstructer(pi[i].ParameterType));
                    }
                }
            }
            return Activator.CreateInstance(c.ReflectedType,para,null);
        }
        object createValueType(ParameterInfo pi, string val) {
            if (pi.ParameterType.IsAssignableFrom(typeof(string))) {
                return (object)val;
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(int))) {
                return (object)int.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(short))) {
                return (object)short.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(long))) {
                return (object)long.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(byte))) {
                return (object)byte.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(ushort))) {
                return (object)ushort.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(uint))) {
                return (object)uint.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(ulong))) {
                return (object)ulong.Parse(val);
            }
            else if (pi.ParameterType.IsAssignableFrom(typeof(sbyte))) {
                return (object)sbyte.Parse(val);
            }
            return null;
        }
        private IIOProvider IO;
        public string replyChannel =  "";

        public override void SendMessage(string message) {
            if (String.IsNullOrWhiteSpace(replyChannel))
                IO.WriteLine(message);
            else
                IO.WriteLine(message, replyChannel);
        }
    }
}
