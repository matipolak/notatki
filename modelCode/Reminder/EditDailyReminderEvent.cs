using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode.Reminder
{
    [Serializable]
    public class EditDailyReminderEvent : Event
    {
        EditDailyReminder EditDailyReminder;

        public EditDailyReminderEvent() : base()
        {
            EditDailyReminder = (EditDailyReminder)parameter;
        }

        public EditDailyReminderEvent(DailyReminder oldDailyReminder, DailyReminder newDailyReminder) : base()
        {
            EditDailyReminder = new EditDailyReminder();
            EditDailyReminder.oldDailyReminder = oldDailyReminder;
            EditDailyReminder.newDailyReminder = newDailyReminder;

            parameter = EditDailyReminder;
        }

        public override void DoEvent(State state)
        {
            int oldIndex = state.DailyReminders.FindIndex(x => x.Name.Equals(EditDailyReminder.oldDailyReminder.Name));
            state.DailyReminders[oldIndex] = EditDailyReminder.newDailyReminder;
            state.lastTimeUpdateReminders = DateTime.Now;
        }

        public override void RevertEvent(State state)
        {
            int oldIndex = state.DailyReminders.FindIndex(x => x.Name.Equals(EditDailyReminder.newDailyReminder.Name));
            state.DailyReminders[oldIndex] = EditDailyReminder.oldDailyReminder;
            state.lastTimeUpdateReminders = DateTime.Now;
        }
    }
}
