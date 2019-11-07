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
using Microsoft.Extensions.Configuration;

namespace SaitCourses.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _db;
        private readonly IConfiguration Configuration;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ApplicationContext db, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
            Configuration = configuration;
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
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackurl = Url.Action("confirmemail",
                        "account",
                        new { userid = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailservice = new EmailService(Configuration);
                    await emailservice.SendEmailAsync(model.Email, "confirm your account",
                        $"to confirm registration, follow the link: <a href='{callbackurl}'>link</a>");

                    return View("RegisterConfirm","Account");
                    
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
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    bool emailConf = await _userManager.IsEmailConfirmedAsync(user);
                    if (emailConf)
                    {
                        bool blocked = await _userManager.GetLockoutEnabledAsync(user);
                        if (blocked)
                        {
                            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                            if (result.Succeeded)
                            {
                                if (_db.Users.Count() == 1)
                                {
                                    await _userManager.AddToRoleAsync(user, "Admin");
                                    await _signInManager.SignInAsync(user, false);
                                }
                                else
                                {
                                    await _userManager.AddToRoleAsync(user, "User");
                                    await _signInManager.SignInAsync(user, false);
                                }
                                return RedirectToAction("Index", "Home");
                            }   
                            else
                                ModelState.AddModelError("", "Incorrect login or password");
                        }
                        else
                        {
                            ModelState.AddModelError("", "You account is blocked");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email wasn't confirm");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }
            return View(model); 
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