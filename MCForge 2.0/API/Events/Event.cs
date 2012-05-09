using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.API.Events {
    /// <summary>
    /// Event class
    /// </summary>
    /// <typeparam name="T1">The type of the sender</typeparam>
    /// <typeparam name="T2">The type of the arguments</typeparam>
    public class Event<T1, T2> where T2 : EventArgs {
        /// <summary>
        /// The delgate type of the events
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The arguments</param>
        public delegate void EventHandler(T1 sender, T2 args);
        internal event EventHandler SystemLvl;
        private List<EventHandler> low = new List<EventHandler>();
        /// <summary>
        /// Low events
        /// </summary>
        public event EventHandler Low {
            add { low.Add(value); }
            remove { low.Remove(value); }
        }
        private List<EventHandler> normal = new List<EventHandler>();
        /// <summary>
        /// Normal events
        /// </summary>
        public event EventHandler Normal {
            add { normal.Add(value); }
            remove { normal.Remove(value); }
        }
        private List<EventHandler> high = new List<EventHandler>();
        /// <summary>
        /// High events
        /// </summary>
        public event EventHandler High {
            add { high.Add(value); }
            remove { high.Remove(value); }
        }
        private List<EventHandler> important = new List<EventHandler>();
        /// <summary>
        /// Important events
        /// </summary>
        public event EventHandler Important {
            add { important.Add(value); }
            remove { important.Remove(value); }
        }
        /// <summary>
        /// Invokes an event
        /// </summary>
        /// <param name="sender">The object invoking the event</param>
        /// <param name="args">The arguments to be passed to the event</param>
        /// <returns>The arguments</returns>
        public T2 Call(T1 sender, T2 args) {
            if (args.GetType().GetInterface("ICloneable") != null && args.GetType().GetInterface("IEquatable`1") != null) {
                return CallCloneable(sender, args);
            }
            else {
                return CallNotCloneable(sender, args);
            }
        }
        /// <summary>
        /// Invokes events of two events in priority stages
        /// </summary>
        /// <param name="sender">The object invoking the event</param>
        /// <param name="args">The argument to be passed to the event</param>
        /// <param name="other">The other event to invoke</param>
        /// <returns>The arguments</returns>
        public T2 Call(T1 sender, T2 args, Event<T1, T2> other) {
            if (args.GetType().GetInterface("ICloneable") != null && args.GetType().GetInterface("IEquatable`1") != null) {
                return CallCloneable(sender, args, other);
            }
            else {
                return CallNotCloneable(sender, args, other);
            }
        }

        private T2 CallNotCloneable(T1 sender, T2 args) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            CallPriorityGroup(important.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!stopped) {
                CallPriorityGroup(high.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (!stopped) {
                    CallPriorityGroup(normal.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (!stopped) {
                        CallPriorityGroup(low.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    }
                }
            }
            if (canceled)
                ((ICancelable)args).Cancel();
            if (!canceled)
                if (SystemLvl != null) SystemLvl(sender, args);
            return args;
        }
        private T2 CallCloneable(T1 sender, T2 args) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            T2 orig = (T2)((ICloneable)args).Clone();
            T2 ret = default(T2);
            CallPriorityGroup(important.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!((IEquatable<T2>)orig).Equals(args))
                ret = (T2)((ICloneable)args).Clone();
            if (!stopped) {
                args = (T2)((ICloneable)orig).Clone();
                CallPriorityGroup(high.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                    ret = (T2)((ICloneable)args).Clone();
                if (!stopped) {
                    args = (T2)((ICloneable)orig).Clone();
                    CallPriorityGroup(normal.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                        ret = (T2)((ICloneable)args).Clone();
                    if (!stopped) {
                        args = (T2)((ICloneable)orig).Clone();
                        CallPriorityGroup(low.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                            ret = (T2)((ICloneable)args).Clone();
                    }
                }
            }
            if (canceled)
                ((ICancelable)ret).Cancel();
            if (!canceled)
                if (SystemLvl != null) SystemLvl(sender, ret);
            return ret;
        }

        private T2 CallNotCloneable(T1 sender, T2 args, Event<T1,T2> other) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            CallPriorityGroup(important.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
            CallPriorityGroup(other.important.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!stopped) {
                CallPriorityGroup(high.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                CallPriorityGroup(other.high.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (!stopped) {
                    CallPriorityGroup(normal.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    CallPriorityGroup(other.normal.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (!stopped) {
                        CallPriorityGroup(low.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        CallPriorityGroup(other.low.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    }
                }
            }
            if (canceled)
                ((ICancelable)args).Cancel();
            if (!canceled)
                if (SystemLvl != null) SystemLvl(sender, args);
            return args;
        }
        private T2 CallCloneable(T1 sender, T2 args, Event<T1,T2> other) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            T2 orig = (T2)((ICloneable)args).Clone();
            T2 ret = default(T2);
            CallPriorityGroup(important.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
            CallPriorityGroup(other.important.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!((IEquatable<T2>)orig).Equals(args))
                ret = (T2)((ICloneable)args).Clone();
            if (!stopped) {
                args = (T2)((ICloneable)orig).Clone();
                CallPriorityGroup(high.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                CallPriorityGroup(other.high.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                    ret = (T2)((ICloneable)args).Clone();
                if (!stopped) {
                    args = (T2)((ICloneable)orig).Clone();
                    CallPriorityGroup(normal.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    CallPriorityGroup(other.normal.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                        ret = (T2)((ICloneable)args).Clone();
                    if (!stopped) {
                        args = (T2)((ICloneable)orig).Clone();
                        CallPriorityGroup(low.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        CallPriorityGroup(other.low.ToArray(), sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                            ret = (T2)((ICloneable)args).Clone();
                    }
                }
            }
            if (canceled)
                ((ICancelable)ret).Cancel();
            if (!canceled)
                if (SystemLvl != null) SystemLvl(sender, ret);
            return ret;
        }
        private T2 CallPriorityGroup(EventHandler[] priorityGroup,T1 sender, T2 args, bool stoppable, ref bool stopped, bool cancelable, ref bool canceled) {
            foreach (EventHandler eh in priorityGroup) {
                eh.Invoke(sender, args);
                if (stoppable && !stopped && ((IStoppable)args).Stopped) stopped = true;
                if (cancelable && !canceled && ((ICancelable)args).Canceled) canceled = true;
            }
            return args;
        }
    }
    /// <summary>
    /// The EventArgs base class
    /// </summary>
    public abstract class EventArgs {
    }
}
