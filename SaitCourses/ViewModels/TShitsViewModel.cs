using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SaitCourses.Models;

namespace SaitCourses.ViewModels
{
    public class TShitsViewModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string TShirtName { get; set; }
        public string image { get; set; }
        public string theme { get; set; }
        //public IEnumerable<Image> images { get; set; }
        public string description { get; set; }
        public double rating { get; set; }
        public bool[] ratings { get; set; }
        public string Topic { get; set; }
        public string[] Topics { get; set; }
    }
}
