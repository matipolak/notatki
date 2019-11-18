using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode.Reminder
{
    [Serializable]
    public class AddDailyReminderEvent : Event
    {
        DailyReminder DailyReminder;

        public AddDailyReminderEvent() : base()
        {
            DailyReminder = (DailyReminder)parameter;
        }

        public AddDailyReminderEvent(DailyReminder DailyReminder) : base()
        {
            this.DailyReminder = DailyReminder;
            parameter = DailyReminder;
        }

        public override void DoEvent(State state)
        {
            state.DailyReminders.Add(DailyReminder);
        }

        public override void RevertEvent(State state)
        {
            state.DailyReminders.RemoveAll((x => x.Name.Equals(DailyReminder.Name)));
        }
    }
}
