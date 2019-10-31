using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class Rating
    {
        public int id { get; set; }
        public int value { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
    }
}

