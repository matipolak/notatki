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
        public String Name;
        public bool Monday;
        public bool Tuesday;
        public bool Wednesday;
        public bool Thursday;
        public bool Friday;
        public bool Saturday;
        public bool Sunday;
    }
}
