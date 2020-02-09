using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode.Reminder
{
    [Serializable]
    public class RemoveDailyReminderEvent : Event
    {
        DailyReminder DailyReminder;

        public RemoveDailyReminderEvent() : base()
        {
            DailyReminder = (DailyReminder)parameter;
        }

        public RemoveDailyReminderEvent(DailyReminder DailyReminder) : base()
        {
            this.DailyReminder = DailyReminder;
            parameter = DailyReminder;
        }

        public override void DoEvent(State state)
        {
            state.DailyReminders.RemoveAll((x => x.Name.Equals(DailyReminder.Name)));
            state.lastTimeUpdateReminders = DateTime.Now;
        }

        public override void RevertEvent(State state)
        {
            state.DailyReminders.Add(DailyReminder);
            state.lastTimeUpdateReminders = DateTime.Now;
        }
    }
}
