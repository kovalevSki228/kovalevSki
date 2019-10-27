using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SaitCourses.Models;

namespace SaitCourses
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            if (await roleManager.FindByNameAsync("Theme") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Theme"));
            }
            if (await roleManager.FindByNameAsync("LangueRu") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("LangueRu"));
            }
            if (await roleManager.FindByNameAsync("LangueEn") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("LangueEn"));
            }
        }
    }
}
