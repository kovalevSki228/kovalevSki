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
        public string userid { get; set; }
        public string image { get; set; }
        public string theme { get; set; }
        //public IEnumerable<Image> images { get; set; }
        public string description { get; set; }
        public string data { get; set; }
        public double rating { get; set; }
        public string sex { get; set; }
        public string size { get; set; }
        public bool[] ratings { get; set; }
        public string Topic { get; set; }
        public string[] Topics { get; set; }
        public string Tag { get; set; }
        public string[] Tegs { get; set; }
        public CommentsView[] comments { get; set; }
    }
    public class CommentsView
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string Text { get; set; }
        public int like { get; set; }
        public int commentId { get; set; }
    }


    public class HomeViewModel
    {
        public IEnumerable<Shirt> shirt { get; set; }
        public IEnumerable<Tag> tag { get; set; }
        public IEnumerable<Topic> topic { get; set; }
    }
}
