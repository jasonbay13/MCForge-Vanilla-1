using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.API
{
    public class EventHelper
    {
        internal static List<EventHelper> cache = new List<EventHelper>();
        public object Delegate;
        public Priority priority;
        public Event type;
        public EventHelper(object Delegate, Priority pri, Event type) { this.Delegate = Delegate; this.priority = pri; this.type = type; }
        public static void Organize()
        {
            //TODO
            //Organize them, Low being called first and System_Level being called last
        }
        public static void Push(EventHelper c)
        {
            cache.Add(c);
            Organize();
        }
    }
}
