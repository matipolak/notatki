using Notatki.modelCode;
using Notatki.modelCode.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Notatki
{
    [XmlInclude(typeof(AddDailyReminderEvent)), 
     XmlInclude(typeof(RemoveDailyReminderEvent)),
     XmlInclude(typeof(EditDailyReminderEvent))]
    [Serializable]
    public abstract class Event
    {
        public Event()
        {
            date = DateTime.Now;
        }

        public EventParameter parameter;
        public DateTime date;
        public abstract void DoEvent(State state);
        public abstract void RevertEvent(State state);
    }
}
