using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class TShirt
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string createDate { get; set; }
        public string userId { get; set; }
        public int themeId { get; set; }
        public string image { get; set; }

        public Theme theme { get; set; }
        public User user { get; set; }
        public Image images { get; set; }
    }
}
