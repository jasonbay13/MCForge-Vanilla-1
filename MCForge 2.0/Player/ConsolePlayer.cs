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
                IO.WriteLine("Properties: ");
                PropertyInfo[] props = args.ArgsType.GetProperties();
                for (int i = 0; i < props.Length; i++) {
                    IO.WriteLine(props[i].Name + " (" + props[i].PropertyType.Name + ")");
                }
            }
        }
        private IIOProvider IO;

        public override void SendMessage(string message) {
            IO.WriteLine(message);
        }
    }
}
