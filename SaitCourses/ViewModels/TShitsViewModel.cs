﻿using System;
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
        public string data { get; set; }
        public double rating { get; set; }
        public bool[] ratings { get; set; }
        public string Topic { get; set; }
        public string[] Topics { get; set; }
        public string Tag { get; set; }
        public string[] Tegs { get; set; }
    }

    public class HomeViewModel
    {
        public IEnumerable<Shirt> shirt { get; set; }
        public IEnumerable<Tag> tag { get; set; }
        public IEnumerable<Topic> topic { get; set; }
    }
}
