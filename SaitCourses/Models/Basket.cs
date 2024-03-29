﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaitCourses.Models
{
    public class Basket
    {
        public int id { get; set; }
        public string nameShirt { get; set; }
        public string dataOfPurchase { get; set; }
        public string userId { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
        public bool purchaseStatus { get; set; }
        public string sex { get; set; }
        public string size { get; set; }
        public int amount { get; set; }
    }
}
