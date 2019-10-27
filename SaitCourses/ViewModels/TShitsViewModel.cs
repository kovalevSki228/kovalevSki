using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
        public string description { get; set; }
    }
}
