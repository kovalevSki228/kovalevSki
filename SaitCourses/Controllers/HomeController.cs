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
        private void Language(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }
        private List<Shirt> Sort(List<Shirt> shirts, string sort)
        {
            switch (sort)
            {
                case "Name up":
                    shirts = shirts.OrderBy(item => item.name).ToList();
                    break;
                case "Name down":
                    shirts = shirts.OrderByDescending(item => item.name).ToList();
                    break;
                case "Data up":
                    shirts = shirts.OrderBy(item => item.createDate).ToList();
                    break;
                case "Data down":
                    shirts = shirts.OrderByDescending(item => item.createDate).ToList();
                    break;
            }
            return shirts;
        }
        private Shirt[] Sort(Shirt[] shirts, string sort)
        {
            switch (sort)
            {
                case "Name up":
                    shirts = shirts.OrderBy(item => item.name).ToArray();
                    break;
                case "Name down":
                    shirts = shirts.OrderByDescending(item => item.name).ToArray();
                    break;
                case "Data up":
                    shirts = shirts.OrderBy(item => item.createDate).ToArray();
                    break;
                case "Data down":
                    shirts = shirts.OrderByDescending(item => item.createDate).ToArray();
                    break;
            }
            return shirts;
        }
        private double[,] SortRating(double[,] shirtTemp, Shirt[] allShirt)
        {
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
            return shirtTemp;
        }
        private double[,] GetRating(double[,] shirtTemp, Shirt[] allShirt)
        {
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
            return shirtTemp;
        }
        private Shirt[] FiveTShirts(Shirt[] shirtsSortRating, Shirt[] allShirt, double[,] shirtTemp)
        {
            for (int j = 0; j < allShirt.Length; j++)
            {
                if (j == 5) break;
                shirtsSortRating[j] = _db.tshirts.FirstOrDefault(item => item.id == shirtTemp[j, 0]);
            }
            return shirtsSortRating;
        }
        private HomeViewModel SearchTopic(string topic, string sort, Shirt[] shirtsSortRating)
        {
            if (!String.IsNullOrEmpty(topic))
            {
                Shirt[] _shirt;
                if (topic != "All")
                {
                    var topics = _db.topics.FirstOrDefault(item => item.nameTopic == topic);
                    var shirtid = _db.tshirts.Where(item => item.themeId == topics.id).ToList();
                    

                    _shirt = new Shirt[shirtid.Count];
                    _shirt = shirtid.ToArray();
                }
                else
                    _shirt = _db.tshirts.Select(item => item).ToArray();
                _shirt = Sort(_shirt, sort);
                return new HomeViewModel
                {
                    shirtsRating = shirtsSortRating.ToList(),
                    shirt = _shirt.ToList(),
                    tag = _db.tags.ToList(),
                    topic = _db.topics.ToList()
                };
            }
            return null;
        }
        private HomeViewModel SearchTag(string tag, string sort, Shirt[] shirtsSortRating)
        {
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
                _shirt = Sort(_shirt, sort);
                return new HomeViewModel
                {
                    shirtsRating = shirtsSortRating.ToList(),
                    shirt = _shirt.ToList(),
                    tag = _db.tags.ToList(),
                    topic = _db.topics.ToList()
                };
            }
            return null;
        }
        private List<Shirt> Search(string search, List<Shirt> shirtSearch)
        {
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
            return shirtSearch;
        }
        private HomeViewModel HomaPage(HomeViewModel homeViewModel, string search, string tag, string sort)
        {
            if (_db.topics.FirstOrDefault(item => item.nameTopic == "All") == null)
                _db.topics.Add(new Topic
                {
                    nameTopic = "All"
                });
            _db.SaveChanges();
            Shirt[] allShirt = _db.tshirts.ToArray();
            double[,] shirtTemp = new double[allShirt.Length, 2];

            shirtTemp = GetRating(shirtTemp, allShirt);
            shirtTemp = SortRating(shirtTemp, allShirt);

            Shirt[] shirtsSortRating = new Shirt[5];
            shirtsSortRating = FiveTShirts(shirtsSortRating, allShirt, shirtTemp);

            if (SearchTag(tag, sort, shirtsSortRating) != null)
                return SearchTag(tag, sort, shirtsSortRating);
            if (SearchTopic(homeViewModel.topics, sort, shirtsSortRating) != null)
                return SearchTopic(homeViewModel.topics, sort, shirtsSortRating);
            List<Shirt> shirtSearch = _db.tshirts.Select(item => item).ToList();
            shirtSearch = Search(search, shirtSearch);
            shirtSearch = Sort(shirtSearch, sort);

            return new HomeViewModel
            {
                shirtsRating = shirtsSortRating.ToList(),
                shirt = shirtSearch.ToList(),
                tag = _db.tags.ToList(),
                topic = _db.topics.ToList()
            };
        }

        public IActionResult Index(HomeViewModel homeViewModel, string search, string tag , string sort)
         {
            return View(HomaPage(homeViewModel,search,tag,sort));
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
            return Ok();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Language(culture);
            return LocalRedirect(returnUrl);
        }
    }
}
