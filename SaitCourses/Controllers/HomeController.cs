using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace SaitCourses.Controllers
{
   
    public class HomeController : Controller
    {
        public UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Upload()
        {
            var files = HttpContext.Request.Form.Files;
            //var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            //foreach (var file in files)
            //{
            //    if (file.Length > 0)
            //    {
            //        var fileName = ContentDispositionHeaderValue.Parse
            //            (file.ContentDisposition).FileName.Trim('"');
            //        System.Console.WriteLine(fileName);
            //        file.SaveAs(Path.Combine(uploads, fileName));
            //    }
            //}

            return Ok();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        //public string GetCulture(string code = "")
        //{
        //    if (!String.IsNullOrEmpty(code))
        //    {
        //        CultureInfo.CurrentCulture = new CultureInfo(code);
        //        CultureInfo.CurrentUICulture = new CultureInfo(code);
        //    }
        //    return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        //}
    }
}
