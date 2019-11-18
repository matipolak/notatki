using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Notatki.modelCode
{
    public class ModelManager
    {
        State actualState;
        History actualHistory;

        public ModelManager()
        {
            actualState = new State();
            actualHistory = new History();
        }

        public void CommitEvent(Event newEvent)
        {
            actualHistory.events.Add(newEvent);
            newEvent.DoEvent(actualState);
        }

        public void SaveModel()
        {
            SaveHistory("history.xml");
            SaveState("state.xml");
        }

        public void LoadModel()
        {
            LoadHistory("history.xml");
            LoadState("state.xml");
        }

        public State GetActualState()
        {
            return actualState;
        }

        private void SaveHistory(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(actualHistory.GetType());
                serializer.Serialize(writer, actualHistory);
                writer.Flush();
            }
        }
        private void LoadHistory(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(History));
                actualHistory = serializer.Deserialize(stream) as History;
            }
        }
        private void SaveState(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(actualState.GetType());
                serializer.Serialize(writer, actualState);
                writer.Flush();
            }
        }
        private void LoadState(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(State));
                actualState = serializer.Deserialize(stream) as State;
            }
        }
    }
}
