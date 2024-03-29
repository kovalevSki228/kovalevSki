﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string userId { get; set; }
        public IEnumerable<Basket> basket { get; set; }
        public string size { get; set; }
        public string sex { get; set; }
        public User user { get; set; }
        public int shirtid { get; set; }
        public Shirt shirt { get; set; }
        public int amount { get; set; }
        public string returnUrl { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string noAutorize { get; set; }
    }
    public class BasketViewModelNoAutentific
    {
        public int id { get; set; }
        public string nameShirt { get; set; }
        public string dataOfPurchase { get; set; }
        public string size { get; set; }
        public string sex { get; set; }
        public string returnUrl { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public NoAutorizeBay noAutorize { get; set; }
    }
}
