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
using SaitCourses.Filters;
using Microsoft.AspNetCore.Identity;

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
        private async Task<User> GetUser()
        {
            User user = await _userManager.GetUserAsync(User);
            return user;
        }
        private async Task<User> GetUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            return user;
        }
        private Comment[] GetComment(User user)
        {
            return _db.comments.Where(item => item.userId == user.Id).ToArray();
        }
        private Like[] GetLike(User user)
        {
            return _db.likes.Where(item => item.userId == user.Id).ToArray();
        }
        private Shirt[] GetShirt(User user)
        {
            return _db.tshirts.Where(item => item.userId == user.Id).ToArray();
        }
        private Achievements Achievemen(Comment[] comment, Like[] getLike, Shirt[] shirt)
        {
            return new Achievements
            {
                comments = comment.Length,
                getLike = getLike.Length,
                setLike = 0,
                shirt = shirt.Length,
                numComments = comment.Length / 5 * 5 + 5,
                numGetLike = getLike.Length / 5 * 5 + 5,
                numSetLike = 5,
                numShirt = shirt.Length / 5 * 5 + 5,
            };
        }
        private EditUserViewModel SettingUser(User user)
        {
            return new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                shirts = _db.tshirts.ToArray(),
                achievements = Achievemen(GetComment(user), GetLike(user), GetShirt(user))
            };
        }
        private async Task<User> SaveSetting(User user, EditUserViewModel model)
        {
            user.Email = model.Email;
            user.UserName = model.Name;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            await _userManager.UpdateAsync(user);
            return null;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View(_userManager.Users.ToList());

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SettingAdmin(string id)
        {
            return View(SettingUser(await GetUser(id)));
        }
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Setting(string id, string returnUrl)
        {

            return View(SettingUser(await GetUser()));
        }
        [TypeFilter(typeof(UserFilters))]
        public IActionResult Constructor()
        {
            string[] topics = _db.topics.Select(item => item.nameTopic).ToArray();

            return View(new TShitsViewModel {
                Topics = topics,
                Tegs = _db.tags.Select(item => item.name).ToArray(),
                tag = _db.tags.ToArray()
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
            var comments = _db.comments.Where(item => item.tShirtId == id).ToArray();
            CommentsView[] commentsView = new CommentsView[comments.Length];
            for (int i = 0; i < comments.Length; i++)
            {
                commentsView[i] = new CommentsView
                {
                    like = comments[i].like,
                    Text = comments[i].text,
                    userName = _db.Users.FirstOrDefault(item =>
                        item.Id == comments[i].userId).UserName,
                    commentId = comments[i].id
                };
            }
            return View(new TShitsViewModel {
                id = result.id,
                description = result.description,
                TShirtName = result.name,
                image = result.image,
                rating = marksRes,
                ratings = marks,
                data = result.createDate,
                comments = commentsView
            });
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> SetRating(TShitsViewModel model, string returnUrl)
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
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        mark.value = i;
                        _db.ratings.Update(mark);
                        await _db.SaveChangesAsync();
                        return Redirect(returnUrl);
                    }
                }
            }
            if (mark != null)
            {
                _db.ratings.Remove(mark);
                await _db.SaveChangesAsync();
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Setting(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await GetUser();
                if (await GetUser() != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Name;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Setting");
                }
            }
            return View();
        }
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Edit(string id)
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
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Edit(EditUserViewModel model)
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

                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    //IdentityResult result =
                    //    await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    //if (result.Succeeded)
                    //{
                    //    user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                    //    await _userManager.UpdateAsync(user);
                    //    return RedirectToAction("Index");
                    //}
                    //else
                    //{
                    //    foreach (var error in result.Errors)
                    //    {
                    //        ModelState.AddModelError(string.Empty, error.Description);
                    //    }
                    //}


                    //var result = await _userManager.UpdateAsync(user);
                    //if(result.Succeeded)
                    //{
                    //    return RedirectToAction("Index");
                    //}
                    //else
                    //{
                    //    foreach (var error in result.Errors)
                    //    {
                    //        ModelState.AddModelError(string.Empty, error.Description);
                    //    }
                    //}
                }
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
               var tempC = _db.comments.Where(item => item.user == user).ToArray();
                for (int i = 0; i < tempC.Length; i++)
                {
                    var res = _db.comments.FirstOrDefault(item => item.user == user);
                    if (res != null)
                        _db.comments.Remove(res);
                }
                var tempR = _db.ratings.Where(item => item.user == user).ToArray();
                for (int i = 0; i < tempR.Length; i++)
                {
                    var res2 = _db.ratings.FirstOrDefault(item => item.user == user);
                    if (res2 != null)
                        _db.ratings.Remove(res2);
                }
                var tempT = _db.tshirts.Where(item => item.users == user).ToArray();
                for (int i = 0; i < tempT.Length; i++)
                {
                    var res3 = _db.tshirts.FirstOrDefault(item => item.users == user);
                    if (res3 != null)
                        _db.tshirts.Remove(res3);
                }
        IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Name = user.Name };
            return View(model);
        }
        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
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
        public async  Task<IActionResult> SetAdmin(string id)
        {
            await _userManager.AddToRoleAsync(await GetUser(id), "Admin");
            return View("Index");
        }
        public async Task<IActionResult> RemAdmin(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, "Admin");
            return View("Index");
        }
    }
}