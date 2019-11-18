using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Notatki.modelCode
{
    [XmlInclude(typeof(DailyReminder))]
    [Serializable]
    public abstract class EventParameter
    {
        public EventParameter()
        {

        }
    }
}
