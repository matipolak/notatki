using Notatki.modelCode.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notatki.modelCode.Achievement
{
    public class Achievement
    {
        public String Name;
        public ReminderType type;
        public int achivedInt;
        public bool Achived;
        public bool Required;

        public Achievement()
        {
            achivedInt = 0;
            type = ReminderType.BOOLEAN;
        }
    }
}
