using Notatki.modelCode.Achievement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Notatki.modelCode
{
    public class AchievementAnalizer
    {

        public static int getRequierCount(DailyAchievements dailyAchievements)
        {
            return dailyAchievements.achievements.FindAll(x => x.Required).Count;
        }

        public static int getAchivedRequierCount(DailyAchievements dailyAchievements)
        {
            return dailyAchievements.achievements.FindAll(x => x.Required && (x.Achived || x.achivedInt > 0)).Count;
        }

        public static int getCount(DailyAchievements dailyAchievements)
        {
            return dailyAchievements.achievements.Count();
        }

        public static int getAchivedCount(DailyAchievements dailyAchievements)
        {
            return dailyAchievements.achievements.FindAll(x => (x.Achived || x.achivedInt > 0)).Count;
        }

    }
}
