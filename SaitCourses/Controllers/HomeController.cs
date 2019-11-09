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
using Microsoft.AspNetCore.Authorization;
using SaitCourses.ViewModels;


namespace SaitCourses.Controllers
{
   
    public class HomeController : Controller
    {
        public UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly ApplicationContext _db;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public IActionResult Index(HomeViewModel homeViewModel, string search, string tag , string sort)
         {
            Shirt[] allShirt = _db.tshirts.ToArray();
            double[,] shirtTemp = new double[allShirt.Length, 2];
            for (int j = 0; j < allShirt.Length; j++)
            {
                shirtTemp[j, 0] = allShirt[j].id;
                int[] resaultRating = _db.ratings.Where(item => item.shirt.id == allShirt[j].id).Select(item => item.value).ToArray();
                double marksRes = 0;
                if (resaultRating.Length > 0)
                {
                    for (int k = 0; k < resaultRating.Length; k++)
                    {
                        marksRes += resaultRating[k] + 1;
                    }
                    marksRes /= resaultRating.Length;
                }
                shirtTemp[j, 1] = marksRes;
            }
            for (int j = 0; j < allShirt.Length; j++)
            {
                for (int k = 0; k < allShirt.Length - 1; k++)
                {
                    if (shirtTemp[k, 1] < shirtTemp[k + 1, 1])
                    {
                        double temp = shirtTemp[k, 1];
                        shirtTemp[k, 1] = shirtTemp[k + 1, 1];
                        shirtTemp[k + 1, 1] = temp;
                        temp = shirtTemp[k, 0];
                        shirtTemp[k, 0] = shirtTemp[k + 1, 0];
                        shirtTemp[k + 1, 0] = temp;
                    }
                }
            }
            Shirt[] shirtsSortRating = new Shirt[5];
            for (int j = 0; j < allShirt.Length; j++)
            {
                if (j == 5) break;
                shirtsSortRating[j] = _db.tshirts.FirstOrDefault(item => item.id == shirtTemp[j, 0]);
            }
            if (!String.IsNullOrEmpty(tag))
            {
                var tags = _db.tags.FirstOrDefault(item => item.name == tag);

                var shirtid = _db.tagInTShirts.Where(item => item.tagid == tags.id).ToList();

                Shirt[] _shirt = new Shirt[shirtid.Count];
                int i = 0;

                foreach (var shId in shirtid)
                {
                    _shirt[i] = _db.tshirts.FirstOrDefault(item => item.id == shId.shirtid);
                    i++;
                }
                switch (homeViewModel.sort)
                {
                    case "Name up": _shirt = _shirt.OrderBy(item => item.name).ToArray();
                        break;
                    case "Name down": _shirt = _shirt.OrderByDescending(item => item.name).ToArray();
                        break;
                    case "Data up":
                        _shirt = _shirt.OrderBy(item => item.createDate).ToArray();
                        break;
                    case "Data down":
                        _shirt = _shirt.OrderByDescending(item => item.createDate).ToArray();
                        break;
                }

                
                return View(new HomeViewModel
                {
                    shirtsRating = shirtsSortRating.ToList(),
                    shirt = _shirt.ToList(),
                    tag = _db.tags.ToList(),
                    topic = _db.topics.ToList()
                });
            }
            var shirtSearch = _db.tshirts.Select(item => item).ToList();
            if (!String.IsNullOrEmpty(search))
            {
                //var commentSearch = _db.comments.Select(item => item);
                //commentSearch = commentSearch.Where(item => item.text.Contains(search) || item.user.UserName.Contains(search));
                //var tagsSearch = _db.tags.Select(item => item);
                //tagsSearch = tagsSearch.Where(item => item.name.Contains(search));
                //var tagSearch = _db.tagInTShirts.Select(item => item).ToArray();
                shirtSearch = shirtSearch.Where(item => item.description.Contains(search) || item.name.Contains(search)).ToList();
                //for (int i = 0; i < commentSearch.Count(); i++)
                //{
                //    if (shirtSearch.FirstOrDefault(item => item.id == commentSearch.) == null)
                //        shirtSearch.Add( _db.tshirts.FirstOrDefault(item => item.id == commentSearch[i].tShirtId));
                //}
                //for (int i = 0; i < tagsSearch.Count(); i++)
                //{
                //    var temp = _db.tagInTShirts.FirstOrDefault(item => item.tagid == tagsSearch[i].id);
                //    if (shirtSearch.FirstOrDefault(item => item.id == temp.shirtid) == null)
                //        shirtSearch.Add(_db.tshirts.FirstOrDefault(item => item.id == temp.shirtid));
                //}

            }
            switch (sort)
            {
                case "Name up":
                    shirtSearch = shirtSearch.OrderBy(item => item.name).ToList();
                    break;
                case "Name down":
                    shirtSearch = shirtSearch.OrderByDescending(item => item.name).ToList();
                    break;
                case "Data up":
                    shirtSearch = shirtSearch.OrderBy(item => item.createDate).ToList();
                    break;
                case "Data down":
                    shirtSearch = shirtSearch.OrderByDescending(item => item.createDate).ToList();
                    break;
            }
            return View(new HomeViewModel
            {
                shirtsRating = shirtsSortRating.ToList(),
                shirt = shirtSearch.ToList(),
                tag = _db.tags.ToList(),
                topic = _db.topics.ToList()
            });
        }

 


        public IActionResult Privacy()
        {
            
            return View(_db.tshirts.ToList());
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
