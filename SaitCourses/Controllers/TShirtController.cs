using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.Models;
using SaitCourses.ViewModels;
using Microsoft.AspNetCore.Identity;
using SaitCourses;
using SaitCourses.Filters;
using Microsoft.AspNetCore.Authorization;

namespace SaitCourses.Controllers
{
    public class TShirtController : Controller
    {
        private readonly ApplicationContext _db;

        
        private readonly UserManager<User> _userManager;
        public TShirtController(ApplicationContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
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
        public IActionResult ShirtEdit(int id)
        {
            
            string[] topics = _db.topics.Select(item => item.nameTopic).ToArray();
            Shirt shirt = _db.tshirts.FirstOrDefault(item => item.id == id);
            return View(new TShitsViewModel
            {
                id = shirt.id,
                TShirtName = shirt.name,
                description = shirt.description,
                Topics = topics,
                Tegs = _db.tags.Select(item => item.name).ToArray()
            });
        }

        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Edit(TShitsViewModel model)
        {
            var topics = _db.topics.FirstOrDefault(item => item.nameTopic == model.Topic);
            User user = await _userManager.GetUserAsync(User);
            string tag = model.Tag.Replace("  ", " ");
            string[] tags = tag.Split(' ');
            
            var result = new Shirt
            {
                image = _db.tshirts.FirstOrDefault(item => item.id == model.id).image,
                name = model.TShirtName,
                description = model.description,
                userId = user.Id,
                themeId = topics.id,
                createDate = DateTime.Now.ToString("MM/dd/yyyy"),
                Sex = model.sex
            };
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
            return View();
        }

        public async Task<IActionResult> basket()
        {
            User user = await _userManager.GetUserAsync(User);
            var _basket = _db.baskets.Where(item => item.userId == user.Id).ToList();
            return View(new BasketViewModel
            {
                userId = user.Id,
                basket = _basket.ToList()
            });
        }

        
        [HttpPost]
        public async Task<IActionResult> basket(int id, BasketViewModel model)
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
            return View(new BasketViewModel { 
                userId = user.Id,
                basket = _basket.ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Bay(int id)
        {
            User user = await _userManager.GetUserAsync(User);
            //var result = _db.tshirts.FirstOrDefault(item => item.id == id);
            var result = _db.baskets.Where(item => item.userId == user.Id && item.purchaseStatus == false).ToList();
            string message = "";
            foreach(var status in result)
            {
                status.purchaseStatus = true;
                message += "\n" + status.nameShirt.ToString() + ", price 15 $";
                _db.baskets.Update(status);
                await _db.SaveChangesAsync();
            }            
            EmailService emailservice = new EmailService();

            await emailservice.SendEmailAsync(user.Email, "Purchase",
                "you bought t-shirts: "+message);
            return View("Bay");
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