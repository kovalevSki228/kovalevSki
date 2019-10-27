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

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
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


        public async Task<IActionResult> Setting(string id)
        {
            User user = await _userManager.GetUserAsync(User);
            //User userr = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Name = user.UserName, FirstName = user.FirstName, LastName = user.LastName };
            return View(model);
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
            return View(model);
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