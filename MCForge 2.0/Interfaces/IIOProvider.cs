using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Interfaces {
    /// <summary>
    /// Provides methods for admin input and output
    /// </summary>
    public interface IIOProvider {
        string ReadLine();
        void WriteLine(string line);
    }
}
