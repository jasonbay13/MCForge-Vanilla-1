using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.API.Events {
    public class Event<T1, T2> where T2 : ICloneable {
        public delegate void EventHandler(T1 sender, T2 args);
        internal event EventHandler SystemLvl;
        private List<EventHandler> low = new List<EventHandler>();
        public event EventHandler Low {
            add { low.Add(value); }
            remove { low.Remove(value); }
        }
        private List<EventHandler> normal = new List<EventHandler>();
        public event EventHandler Normal {
            add { normal.Add(value); }
            remove { normal.Remove(value); }
        }
        private List<EventHandler> high = new List<EventHandler>();
        public event EventHandler High {
            add { high.Add(value); }
            remove { high.Remove(value); }
        }
        private List<EventHandler> important = new List<EventHandler>();
        public event EventHandler Important {
            add { important.Add(value); }
            remove { important.Remove(value); }
        }
        /// <summary>
        /// Invokes an event
        /// </summary>
        /// <param name="sender">The object invoking the event</param>
        /// <param name="args">The arguments to be passed to the event</param>
        /// <returns>Whether the handling should be canceled or not</returns>
        public bool Call(T1 sender, T2 args) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            EventHandler[] tmp = important.ToArray();
            foreach (EventHandler eh in tmp) {
                T2 newArgs = (T2)args.Clone();
                eh.Invoke(sender, newArgs);
                if (stoppable && !stopped && ((IStoppable)newArgs).Stopped) stopped = true;
                if (cancelable && !canceled && ((ICancelable)newArgs).Canceled) canceled = true;
            }
            if (!stopped) {
                tmp = high.ToArray();
                foreach (EventHandler eh in tmp) {
                    T2 newArgs = (T2)args.Clone();
                    eh.Invoke(sender, newArgs);
                    if (stoppable && !stopped && ((IStoppable)newArgs).Stopped) stopped = true;
                    if (cancelable && !canceled && ((ICancelable)newArgs).Canceled) canceled = true;
                }
                if (!stopped) {
                    tmp = normal.ToArray();
                    foreach (EventHandler eh in tmp) {
                        T2 newArgs = (T2)args.Clone();
                        eh.Invoke(sender, newArgs);
                        if (stoppable && !stopped && ((IStoppable)newArgs).Stopped) stopped = true;
                        if (cancelable && !canceled && ((ICancelable)newArgs).Canceled) canceled = true;
                    }
                    if (!stopped) {
                        tmp = low.ToArray();
                        foreach (EventHandler eh in tmp) {
                            T2 newArgs = (T2)args.Clone();
                            eh.Invoke(sender, newArgs);
                            if (stoppable && !stopped && ((IStoppable)newArgs).Stopped) stopped = true;
                            if (cancelable && !canceled && ((ICancelable)newArgs).Canceled) canceled = true;
                        }
                    }
                }
            }
            if (!canceled)
                if (SystemLvl != null) SystemLvl(sender, args);
            return canceled;
        }
    }
    public abstract class EventArgs : ICloneable{

        public abstract object Clone();
    }
}
