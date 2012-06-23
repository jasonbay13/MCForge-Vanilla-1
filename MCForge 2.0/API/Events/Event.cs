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
            add {
                low.Add(value);
                if(OnRegister!=null) OnRegister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, true));
            }
            remove {
                low.Remove(value);
                if (OnUnregister != null) OnUnregister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, false));
            }
        }
        private List<EventHandler> normal = new List<EventHandler>();
        /// <summary>
        /// Normal events
        /// </summary>
        public event EventHandler Normal {
            add {
                normal.Add(value);
                if(OnRegister!=null) OnRegister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, true));
            }
            remove {
                normal.Remove(value);
                if (OnUnregister != null) OnUnregister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, false));
            }
        }
        private List<EventHandler> high = new List<EventHandler>();
        /// <summary>
        /// High events
        /// </summary>
        public event EventHandler High {
            add {
                high.Add(value);
                if(OnRegister!=null) OnRegister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, true));
            }
            remove {
                high.Remove(value);
                if (OnUnregister != null) OnUnregister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, false));
            }
        }
        private List<EventHandler> important = new List<EventHandler>();
        /// <summary>
        /// Important events
        /// </summary>
        public event EventHandler Important {
            add {
                important.Add(value);
               if(OnRegister!=null) OnRegister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, true));
            }
            remove {
                important.Remove(value);
                if (OnUnregister != null) OnUnregister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, false));
            }
        }
        /// <summary>
        /// Represents normal priority level (for removing only)
        /// </summary>
        public event EventHandler All {
            add { }
            remove {
                normal.Remove(value);
                if (OnUnregister != null) OnUnregister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), value, false));
            }
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
            CallPriorityGroup(important, sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!stopped) {
                CallPriorityGroup(high, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (!stopped) {
                    CallPriorityGroup(normal, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (!stopped) {
                        CallPriorityGroup(low, sender, args, stoppable, ref stopped, cancelable, ref canceled);
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
            CallPriorityGroup(important, sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!((IEquatable<T2>)orig).Equals(args))
                ret = (T2)((ICloneable)args).Clone();
            if (!stopped) {
                args = (T2)((ICloneable)orig).Clone();
                CallPriorityGroup(high, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                    ret = (T2)((ICloneable)args).Clone();
                if (!stopped) {
                    args = (T2)((ICloneable)orig).Clone();
                    CallPriorityGroup(normal, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                        ret = (T2)((ICloneable)args).Clone();
                    if (!stopped) {
                        args = (T2)((ICloneable)orig).Clone();
                        CallPriorityGroup(low, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args))
                            ret = (T2)((ICloneable)args).Clone();
                    }
                }
            }
            if (ret == default(T2)) ret = orig;
            if (canceled)
                ((ICancelable)ret).Cancel();
            if (!canceled)
                if (SystemLvl != null) SystemLvl(sender, ret);
            return ret;
        }

        private T2 CallNotCloneable(T1 sender, T2 args, Event<T1, T2> other) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            CallPriorityGroup(important, sender, args, stoppable, ref stopped, cancelable, ref canceled);
            other.CallPriorityGroup(other.important, sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!stopped) {
                CallPriorityGroup(high, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                other.CallPriorityGroup(other.high, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (!stopped) {
                    CallPriorityGroup(normal, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    other.CallPriorityGroup(other.normal, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (!stopped) {
                        CallPriorityGroup(low, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        other.CallPriorityGroup(other.low, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    }
                }
            }
            if (canceled)
                ((ICancelable)args).Cancel();
            if (!canceled) {
                if (SystemLvl != null) SystemLvl(sender, args);
                if (other.SystemLvl != null) other.SystemLvl(sender, args);
            }
            return args;
        }
        private T2 CallCloneable(T1 sender, T2 args, Event<T1, T2> other) {
            bool canceled = false;
            bool cancelable = args.GetType().GetInterface("ICancelable") != null;
            bool stopped = false;
            bool stoppable = args.GetType().GetInterface("IStoppable") != null;
            T2 orig = (T2)((ICloneable)args).Clone();
            T2 ret = default(T2);
            CallPriorityGroup(important, sender, args, stoppable, ref stopped, cancelable, ref canceled);
            other.CallPriorityGroup(other.important, sender, args, stoppable, ref stopped, cancelable, ref canceled);
            if (!((IEquatable<T2>)orig).Equals(args)) {
                ret = (T2)((ICloneable)args).Clone();
                args = (T2)((ICloneable)args).Clone();
            }
            if (!stopped) {
                CallPriorityGroup(high, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                other.CallPriorityGroup(other.high, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args)) {
                    ret = (T2)((ICloneable)args).Clone();
                    args = (T2)((ICloneable)args).Clone();
                }
                if (!stopped) {
                    CallPriorityGroup(normal, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    other.CallPriorityGroup(other.normal, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                    if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args)) {
                        ret = (T2)((ICloneable)args).Clone();
                        args = (T2)((ICloneable)args).Clone();
                    }
                    if (!stopped) {
                        CallPriorityGroup(low, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        other.CallPriorityGroup(other.low, sender, args, stoppable, ref stopped, cancelable, ref canceled);
                        if (ret != default(T2) && !((IEquatable<T2>)orig).Equals(args)) {
                            ret = (T2)((ICloneable)args).Clone();
                            args = (T2)((ICloneable)args).Clone();
                        }
                    }
                }
            }
            if (ret == default(T2)) ret = orig;
            if (canceled)
                ((ICancelable)ret).Cancel();
            if (!canceled) {
                if (SystemLvl != null) SystemLvl(sender, ret);
                if (other.SystemLvl != null) other.SystemLvl(sender, ret);
            }
            return ret;
        }
        private T2 CallPriorityGroup(List<EventHandler> priorityGroup, T1 sender, T2 args, bool stoppable, ref bool stopped, bool cancelable, ref bool canceled) {

            foreach (EventHandler eh in priorityGroup.ToArray()) {
                eh.Invoke(sender, args);
                if (stoppable && !stopped && ((IStoppable)args).Stopped) stopped = true;
                if (cancelable && !canceled && ((ICancelable)args).Canceled) canceled = true;
                if (args.Disable) {
                    priorityGroup.Remove(eh);
                    if (OnUnregister != null) OnUnregister.Invoke(this, new EventRegisterArgs(typeof(T1), typeof(T2), eh, false));
                    args.Disable = false;
                }
            }
            return args;
        }
        internal EventHandler<EventRegisterArgs> OnRegister;
        internal EventHandler<EventRegisterArgs> OnUnregister;
    }
    /// <summary>
    /// The EventRegister EventArgs
    /// </summary>
    public class EventRegisterArgs : System.EventArgs {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="senderType">The type of the sender</param>
        /// <param name="argsType">The type of the arguments</param>
        /// <param name="method">The method</param>
        /// <param name="registering">Whether the method gets regestered (true) or unregistered (false)</param>
        public EventRegisterArgs(Type senderType,Type argsType, Delegate method, bool registering) {
            this.SenderType = senderType;
            this.ArgsType = argsType;
            this.Method = method;
            this.Registering = registering;
        }
        /// <summary>
        /// The type of the sender
        /// </summary>
        public Type SenderType;
        /// <summary>
        /// The type of the arguments
        /// </summary>
        public Type ArgsType;
        /// <summary>
        /// The method
        /// </summary>
        public Delegate Method;
        /// <summary>
        /// Whether the method gets regestered (true) or unregistered (false)
        /// </summary>
        public bool Registering;
    }
    /// <summary>
    /// The EventArgs base class
    /// </summary>
    public abstract class EventArgs {
        /// <summary>
        /// Gets or sets whether the event should be disabled for this method, or not.
        /// </summary>
        public bool Disable = false;
        /// <summary>
        /// Unregisters the current invoked eventhandler form this event.
        /// </summary>
        public void Unregister() {
            Disable = true;
        }
    }
}
