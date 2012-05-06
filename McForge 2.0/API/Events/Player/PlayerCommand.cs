using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    public class PlayerCommand:Event<Player,PlayerCommandEventArgs>{
    }
    public class PlayerCommandEventArgs : EventArgs, ICloneable, ICancelable {
        public PlayerCommandEventArgs(string command, string[] args) {
            this.Command = command;
            this.Args = args;
        }
        public string Command;
        public string[] Args;

        public override object Clone() {
            return new PlayerCommandEventArgs(Command, Args);
        }
        private bool canceled = false;
        public bool Canceled {
            get { return canceled; }
        }

        public void Cancel() {
            canceled = true;
        }

        public void Allow() {
            canceled = false;
        }
    }
}
