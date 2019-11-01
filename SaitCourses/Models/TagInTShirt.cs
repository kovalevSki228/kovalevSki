using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class TagInTShirt
    {
        public int id { get; set; }
        public int tagid { get; set; }
        public Tag tag { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
    }
}
