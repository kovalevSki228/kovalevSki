using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SaitCourses.Models;
using Microsoft.AspNetCore.Identity;

namespace SaitCourses.ViewModels
{
    public class EditUserViewModel
    {
        
        public string Id { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "Sex")]
        public bool Sex { get; set; }
        [Display(Name = "OldPassword")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Ppassword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "PasswordConfim")]
        [Compare("Password", ErrorMessage = "Password miss match")]
        [DataType(DataType.Password)]
        public string PasswordConfim { get; set; }
        public Shirt[] shirts { get; set; }
        public int idImg { get; set; }
        public string image { get; set; }
        public string nameImg { get; set; }
        public string description { get; set; }

    }
}
