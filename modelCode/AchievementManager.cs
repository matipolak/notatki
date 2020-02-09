using Notatki.modelCode.Achievement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Notatki.modelCode
{
    public class AchievementManager
    {

        public static void SaveAchievement(string FileName, DailyAchievements dailyAchievements)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(dailyAchievements.GetType());
                serializer.Serialize(writer, dailyAchievements);
                writer.Flush();
            }
        }

        public static DailyAchievements LoadAchievementl(DateTime date)
        {
            String fileName = "achievements/" + DateToString(date) + ".xml";
            DailyAchievements dailyAchievements;
            if (File.Exists(fileName))
            {
                using (var stream = System.IO.File.OpenRead(fileName))
                {
                    var serializer = new XmlSerializer(typeof(DailyAchievements));
                    dailyAchievements = serializer.Deserialize(stream) as DailyAchievements;
                }
            }
            else
            {
                dailyAchievements = new DailyAchievements();
                dailyAchievements.date = date;
                dailyAchievements.achievements = new List<Achievement.Achievement>();
            }
            return dailyAchievements;
        }

        public static String DateToString(DateTime date)
        {
            return date.ToString("dd MMMM yyyy", new CultureInfo("en-GB"));
        }

        public static DateTime StringToDate(String date)
        {
            return DateTime.ParseExact(date, "dd MMMM yyyy", new CultureInfo("en-GB"));
        }


    }
}
