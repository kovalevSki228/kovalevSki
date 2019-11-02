using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class AchievementsUsers
    {
        public int id { get; set; }
        public int achievementid { get; set; }
        public string userid { get; set; }
        public int achievementRequirements { get; set; }
        public int progressUser { get; set; }
        public string image { get; set; }
    }
}
