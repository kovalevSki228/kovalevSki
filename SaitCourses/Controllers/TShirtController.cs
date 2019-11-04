using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.Models;
using SaitCourses.ViewModels;
using Microsoft.AspNetCore.Identity;
using SaitCourses;

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
        //public async Task<IActionResult> Sort(SortShirt sortOrder = SortShirt.NameAsc)
        //{
        //    //IQueryable<User> users = db.Users.Include(x => x.Company);


        //    IQueryable<Shirt> shirt = _db.tshirts.in;

        //    ViewData["NameSort"] = sortShirt == SortShirt.NameAsc ? SortShirt.NameDesc : SortShirt.NameAsc;
        //    ViewData["DataSort"] = sortShirt == SortShirt.DataAsc ? SortShirt.DataDesc : SortShirt.DataAsc;
        //    ViewData["RatingSort"] = sortShirt == SortShirt.RatingAsc ? SortShirt.RatingDesc : SortShirt.RatingAsc;

        //    switch (sortShirt)
        //    {
        //        case SortShirt.NameDesc:
        //            shirt = _db.tshirts.OrderByDescending(item => item.name);
        //            // users = users.OrderByDescending(s => s.Name);
        //            break;
        //        case SortShirt.DataAsc:
        //            shirt = _db.tshirts.OrderBy(item => item.id);
        //            //   users = users.OrderBy(s => s.Age);
        //            break;
        //        case SortShirt.DataDesc:
        //            shirt = _db.tshirts.OrderByDescending(item => item.id);
        //            //  users = users.OrderByDescending(s => s.Age);
        //            break;
        //        case SortShirt.RatingAsc:
        //            //   users = users.OrderBy(s => s.Company.Name);
        //            break;
        //        case SortShirt.RatingDesc:
        //            //    users = users.OrderByDescending(s => s.Company.Name);
        //            break;
        //        default:
        //            shirt = _db.tshirts.OrderBy(item => item.name);
        //            //  users = users.OrderBy(s => s.Name);
        //            break;
        //    }

        //    return View(await shirt.AsNoTracking().ToListAsync());
        //}
        public IActionResult Index() => View(_db.images.ToList());
        public IActionResult EditCampaign()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> EditCampaign(TShitsViewModel model)
        {
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        

    }
}