using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SaitCourses.Models;
using SaitCourses.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SaitCourses.Filters;

namespace SaitCourses.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        ApplicationContext _db;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View(_db.topics.ToList());
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string name)
        {

            var topic = _db.topics.FirstOrDefault(item => item.nameTopic == name);
            if (topic == null)
            {
                _db.topics.Add(new Topic { nameTopic = name });
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}