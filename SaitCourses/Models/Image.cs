﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class Image
    {
        public int id { get; set; }
        
        public string image { get; set; }
        
        public int tShirtId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        //  public TShirt tShirt { get; set; }
    }
}
