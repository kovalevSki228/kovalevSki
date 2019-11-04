﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string text { get; set; }
        public int like { get; set; }
        public string userId { get; set; }
        public User user { get; set; }
        public int tShirtId { get; set; }
        public Shirt tShirt { get; set; }
    }
}
