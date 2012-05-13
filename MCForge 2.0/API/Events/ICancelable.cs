using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.API.Events {
    public interface ICancelable {
        /// <summary>
        /// Whether or not the handling should be canceled
        /// </summary>
        bool Canceled { get; }
        /// <summary>
        /// Cancels the handling
        /// </summary>
        void Cancel();
        /// <summary>
        /// Allows the handling
        /// </summary>
        void Allow();
    }
}
