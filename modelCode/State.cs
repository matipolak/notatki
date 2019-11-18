using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode
{
    [Serializable]
    public class State
    {
        public List<DailyReminder> DailyReminders;
        public State()
        {
            DailyReminders = new List<DailyReminder>();
        }
    }
}
