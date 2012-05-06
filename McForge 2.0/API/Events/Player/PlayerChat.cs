using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;

namespace MCForge.API.Events {
    public class PlayerChat :Event<Player,PlayerChatEventArgs> {
    }
    public class PlayerChatEventArgs : EventArgs, ICloneable, ICancelable {
        public PlayerChatEventArgs(string message) {
            this.Message = message;
        }
        public string Message;
        public override object Clone() {
            return new PlayerChatEventArgs(Message);
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
