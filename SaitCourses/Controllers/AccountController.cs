using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SaitCourses.ViewModels;
using SaitCourses.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace SaitCourses.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name};
                //add user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {



                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackurl = Url.Action("confirmemail",
                    //    "account",
                    //    new { userid = user.Id, code = code },
                    //    protocol: HttpContext.Request.Scheme);
                    //EmailService emailservice = new EmailService();
                    //await emailservice.SendEmailAsync(model.Email, "confirm your account",
                    //    $"to confirm registration, follow the link: <a href='{callbackurl}'>link</a>"))                        ;

                    //return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");



                    ////instal coocks
                    await _userManager.AddToRoleAsync(user, "user");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("index", "home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Name);
                //if (user != null)
                //{
                //    if (!await _userManager.IsEmailConfirmedAsync(user))
                //    {
                //        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                //        return View(model);
                //    }
                //}

                var result =
                    await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid client");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> SwitchColor()
        {
            User user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Theme"))
                await _userManager.RemoveFromRoleAsync(user, "Theme");
            else await _userManager.AddToRoleAsync(user, "Theme");
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Setting", "Users");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}