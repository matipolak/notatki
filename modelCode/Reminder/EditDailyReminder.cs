using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode
{
    [Serializable]
    public class EditDailyReminder : EventParameter
    {
        public DailyReminder oldDailyReminder;
        public DailyReminder newDailyReminder;

        public EditDailyReminder()
        {
            oldDailyReminder = new DailyReminder();
            newDailyReminder = new DailyReminder();
        }
    }
}
