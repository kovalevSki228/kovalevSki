using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaitCourses.Models;

namespace SaitCourses.Views.Users
{
    public class TopicTShirts
    {
        public int id { get; set; }
        public int temeid { get; set; }
        public Topic topic { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
    }
}
