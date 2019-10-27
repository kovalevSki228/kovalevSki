using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SaitCourses.ViewModels
{
    public class ProfileSetting
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LasName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Sex")]
        public bool Sex { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password miss match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confim password")]
        public string PasswordConfim { get; set; }
    }
}
