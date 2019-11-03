using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class Like
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int like { get; set; }
        public int shirtId { get; set; }
    }
}
