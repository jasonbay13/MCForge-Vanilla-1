using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.API
{
    public interface Cancelable
    {
        bool IsCanceled { get; }
        void Cancel(bool value);
    }
}
