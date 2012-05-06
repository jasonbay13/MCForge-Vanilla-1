using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.API.Events {
    public interface IStoppable {
        bool Stopped { get; }
        void Stop();
        void Continue();
    }
}
