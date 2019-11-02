using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaitCourses.Models;

namespace SaitCourses.ViewModels
{
    public class BasketViewModel
    {
        public int id { get; set; }
        public string nameShirt { get; set; }
        public string dataOfPurchase { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
        public int amount { get; set; }
    }
}
