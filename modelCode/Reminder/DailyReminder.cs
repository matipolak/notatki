using Notatki.modelCode.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode
{
    [Serializable]
    public class DailyReminder : EventParameter
    {
        public DailyReminder()
        {
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            Sunday = false;
            type = ReminderType.BOOLEAN;
        }

        public String Name;
        public bool Monday;
        public bool Tuesday;
        public bool Wednesday;
        public bool Thursday;
        public bool Friday;
        public bool Saturday;
        public bool Sunday;
        public ReminderType type;

        public bool Equals(DailyReminder reminder)
        {
            return Name.Equals(reminder.Name)
                && Monday == reminder.Monday
                && Tuesday == reminder.Tuesday
                && Wednesday == reminder.Wednesday
                && Thursday == reminder.Thursday
                && Friday == reminder.Friday
                && Saturday == reminder.Saturday
                && Sunday == reminder.Sunday
                && type == reminder.type;
        }
    }
}
