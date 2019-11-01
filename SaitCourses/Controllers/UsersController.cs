﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.Models;
using SaitCourses.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SaitCourses.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _db;

        public UsersController(UserManager<User> userManager, ApplicationContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View(_userManager.Users.ToList());

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            User user = new User { Email = model.Email, UserName = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> SettingAdmin(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            //User userr = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Name = user.UserName, FirstName = user.FirstName, LastName = user.LastName, shirts = _db.tshirts.ToArray() };
            return View(model);
        }

        public async Task<IActionResult> Setting(string id)
        {
            User user = await _userManager.GetUserAsync(User);
            //User userr = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Name = user.UserName, FirstName = user.FirstName, LastName = user.LastName, shirts = _db.tshirts.ToArray()};
            return View(model);
        }
        [Authorize]
        public IActionResult Constructor()
        {
            string[] topics = _db.topics.Select(item => item.nameTopic).ToArray();

            return View(new TShitsViewModel { 
                TShirtName = "name",
                description = "description",
                Topics = topics,
                Tegs = _db.tags.Select(item => item.name).ToArray()
            });
        }



        public async Task<IActionResult> More(int id)
        {

            var result = await _db.tshirts.FindAsync(id);
            int[] resaultRating = _db.ratings.Where(item => item.shirt.id == id).Select(item => item.value).ToArray();
            double marksRes = 0;
            if (resaultRating.Length > 0)
            {
                for (int i = 0; i < resaultRating.Length; i++)
                {
                    marksRes += resaultRating[i] + 1;
                }
                marksRes /= resaultRating.Length;
            }
            User user = await _userManager.GetUserAsync(User);
            marksRes = Math.Round(marksRes, 2);
            Rating mark;
            bool[] marks = new bool[5];
            if (User.Identity.IsAuthenticated)
            {
                mark = _db.ratings.FirstOrDefault(item => item.shirt.id == id && item.user == user);
                
                if (mark != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i <= mark.value)
                            marks[i] = true;
                        else
                            marks[i] = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        marks[i] = false;
                    }
                }
            }
            return View(new TShitsViewModel { id = result.id, description = result.description, TShirtName = result.name, image = result.image, rating = marksRes, ratings = marks, data = result.createDate });
        }
        [HttpPost]
        public async Task<IActionResult> Block(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
            }                    
            return RedirectToAction("Index");
        }
        [HttpPost]
       // [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> UnBlock(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                
                await _userManager.SetLockoutEnabledAsync(user, false);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SetRating(TShitsViewModel model)
        {
            User user = await _userManager.GetUserAsync(User);
            Shirt shirt = _db.tshirts.FirstOrDefault(item => item.id == model.id);
            Rating mark = _db.ratings.FirstOrDefault(item => item.shirt == shirt && item.user == user);
            for (int i = 4; i >= 0; i--)
            {
                if (model.ratings[i] == true)
                {
                    if (mark == null)
                    {
                        _db.ratings.Add(new Rating { shirt = shirt, user = user, value = i });
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        mark.value = i;
                        _db.ratings.Update(mark);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            if (mark != null)
            {
                _db.ratings.Remove(mark);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Setting(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                //User userr = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Name;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.GetUserAsync(User);
            //User userr = await _userManager.FindByIdAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel {Id = user.Id, Email = user.Email, Name = user.UserName, FirstName = user.FirstName, LastName = user.LastName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                //User userr = await _userManager.FindByIdAsync(model.Id);
                if(user!=null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Name;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    

                    var result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user!=null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Name = user.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if(user!=null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if(result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                
            }
            return View(model);
        }
    }
}