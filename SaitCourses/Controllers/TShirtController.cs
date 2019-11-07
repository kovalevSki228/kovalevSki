using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.Models;
using SaitCourses.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Web;
using SaitCourses;
using SaitCourses.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace SaitCourses.Controllers
{
    public class TShirtController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IConfiguration Configuration;

        private readonly UserManager<User> _userManager;
        private string clientIP()
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            string ip = heserver.AddressList[1].ToString();
            return ip;
        }
        public TShirtController(ApplicationContext db, UserManager<User> userManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            Configuration = configuration;
        }
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Delete(int id)
        {
            var rating = _db.ratings.FirstOrDefault(item => item.shirtid == id);
            var tshirt = await _db.tshirts.FindAsync(id);
            if (rating != null)
            {
                _db.ratings.Remove(rating);
                await _db.SaveChangesAsync();
            }
            if (tshirt != null)
            {
                _db.tshirts.Remove(tshirt);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Setting", "Users");
        }
        public IActionResult CreateTopic() => View();
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTopic(string name)
        {

            var topic = _db.topics.FirstOrDefault(item => item.nameTopic == name);
            if (topic == null)
            {
                _db.topics.Add(new Topic { nameTopic = name });
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Constructor(int id, string returnUrl)
        {
            string[] topics = _db.topics.Select(item => item.nameTopic).ToArray();

            if (String.IsNullOrEmpty(returnUrl))
            {
                return View(new TShitsViewModel
                {
                    Topics = topics,
                    Tegs = _db.tags.Select(item => item.name).ToArray(),
                    tag = _db.tags.ToArray()
                });
            }
            else
            {
                Shirt shirt = await _db.tshirts.FindAsync(id);
                return View(new TShitsViewModel
                {
                    returnUrl = returnUrl,
                    id = shirt.id,
                    image = shirt.image,
                    TShirtName = shirt.name,
                    description = shirt.description,
                    Topics = topics,
                    Tegs = _db.tags.Select(item => item.name).ToArray()
                });
            }
        }

        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Edit(TShitsViewModel model, string returnUrl)
        {
            var topics = _db.topics.FirstOrDefault(item => item.nameTopic == model.Topic);
            User user = await _userManager.GetUserAsync(User);
            string tag = model.Tag.Replace("  ", " ");
            string[] tags = tag.Split(' ');

            var result = _db.tshirts.FirstOrDefault(item => item.id == model.id);

            result.image = _db.tshirts.FirstOrDefault(item => item.id == model.id).image;
            result.name = model.TShirtName;
            result.description = model.description;
            result.userId = user.Id;
            result.themeId = topics.id;
            result.createDate = DateTime.Now.ToString("MM/dd/yyyy");
            result.Sex = model.sex;
             _db.tshirts.Update(result);
            //}
            await _db.SaveChangesAsync();
            for (int i = 0; i < tags.Length; i++)
            {
                if (_db.tags.FirstOrDefault(item => item.name == tags[i]) == null)
                {
                    _db.tags.Add(new Tag { name = tags[i] });
                    await _db.SaveChangesAsync();
                }
                Shirt shirt = _db.tshirts.FirstOrDefault(item => item.name == model.TShirtName);
                Tag tag1 = _db.tags.FirstOrDefault(item => item.name == tags[i]);
                _db.tagInTShirts.Add(new TagInTShirt
                {
                    shirtid = shirt.id,
                    tagid = tag1.id
                });
                await _db.SaveChangesAsync();
            }
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Stores()
        {
            User user = await _userManager.GetUserAsync(User);
            var _basket = _db.baskets.Where(item => item.userId == user.Id).ToList();
            return View(new BasketViewModel
            {
                userId = user.Id,
                basket = _basket.ToList()
            }) ;
        }
        [HttpGet]
        public async Task<IActionResult> basket()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.GetUserAsync(User);
                var _basket = _db.baskets.Where(item => item.userId == user.Id).ToList();
                return View(new BasketViewModel
                {
                    userId = user.Id,
                    basket = _basket.ToList()
                });
            }
            else
            {
                var _basket = _db.baskets.Where(item => item.userId == clientIP()).ToList();
                return View(new BasketViewModel
                {
                    userId = clientIP(),
                    basket = _basket.ToList()
                });
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> basket(int id, BasketViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.GetUserAsync(User);
                var result = _db.tshirts.FirstOrDefault(item => item.id == id);
                _db.baskets.Add(new Basket
                {
                    dataOfPurchase = DateTime.Now.ToString("MM/dd/yyyy"),
                    nameShirt = result.name,
                    amount = 1,
                    shirtid = result.id,
                    userId = user.Id,
                    purchaseStatus = false,
                    sex = model.sex,
                    size = model.size

                });
                await _db.SaveChangesAsync();
                var _basket = _db.baskets.Where(item => item.userId == user.Id).ToList();
                return View(new BasketViewModel
                {
                    userId = user.Id,
                    basket = _basket.ToList()
                });
            }
            else
            {
                var result = _db.tshirts.FirstOrDefault(item => item.id == id);
                _db.baskets.Add(new Basket
                {
                    dataOfPurchase = DateTime.Now.ToString("MM/dd/yyyy"),
                    nameShirt = result.name,
                    amount = 1,
                    shirtid = result.id,
                    userId = clientIP(),
                    purchaseStatus = false,
                    sex = model.sex,
                    size = model.size

                });
                await _db.SaveChangesAsync();
                var _basket = _db.baskets.Where(item => item.userId == clientIP()).ToList();
                return View(new BasketViewModel
                {
                    userId = clientIP(),
                    basket = _basket.ToList()
                });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Bay(BasketViewModel model)
        {

            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.GetUserAsync(User);
                var result = _db.baskets.Where(item => item.userId == user.Id && item.purchaseStatus == false).ToList();
                string message = "";
                foreach (var status in result)
                {
                    status.purchaseStatus = true;
                    message += "\n" + status.nameShirt.ToString() + ", price 30 $";
                    _db.baskets.Update(status);
                    await _db.SaveChangesAsync();
                }
                EmailService emailservice = new EmailService(Configuration);

                await emailservice.SendEmailAsync(user.Email, "Purchase",
                    "you bought t-shirts: " + message);
                return View("Bay");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var result = _db.baskets.Where(item => item.userId == clientIP() && item.purchaseStatus == false).ToList();
                    string message = "";
                    foreach (var status in result)
                    {
                        status.purchaseStatus = true;
                        message += "\n" + status.nameShirt.ToString() + ", price 30 $";
                        _db.baskets.Update(status);
                        await _db.SaveChangesAsync();
                    }
                    EmailService emailservice = new EmailService(Configuration);

                    await emailservice.SendEmailAsync(model.Email, "Purchase",
                        "you bought t-shirts: " + message);
                    return View("Bay");
                }
                var _basket = _db.baskets.Where(item => item.userId == clientIP()).ToList();
                return View("Basket", new BasketViewModel
                {
                    userId = clientIP(),
                    basket = _basket.ToList()                    
                });
            }
            
        }

        [TypeFilter(typeof(UserFilters))]
        public IActionResult Index() => View(_db.images.ToList());
        [TypeFilter(typeof(UserFilters))]
        public IActionResult EditShirt()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> EditShirt(TShitsViewModel model)
        {
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        

    }
}