using System;
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
        public int userId { get; set; }
        public User user { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
        public bool purchaseStatus { get; set; }
        public int amount { get; set; }
    }
}
