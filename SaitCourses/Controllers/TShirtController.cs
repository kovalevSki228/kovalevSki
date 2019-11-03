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

        public IActionResult Index() => View(_db.images.ToList());
        public IActionResult EditCampaign()
        {
            //if (model.campaigns != null)
            //{
            //    for (int i = 0; i < model.campaigns.Length; i++)
            //    {
            //        if (model.campaigns[i].selected)
            //        {
            //            Campaign Campaign = _db.Campaigns.FirstOrDefault(item => item.id == model.campaigns[i].id);
            //            string[] themes = _db.Themes.Select(item => item.name).ToArray();
            //            string[] tags = _db.TagsToCampaigns.Where(item => item.campaign == Campaign).Select(item => item.tag.name).ToArray();
            //            string tag = "";
            //            foreach (string buf in tags)
            //            {
            //                tag += buf + " ";
            //            }
            //            return View(new CampaignViewModel
            //            {
            //                id = Campaign.id,
            //                CampaignInfo = Campaign.description,
            //                CampaignName = Campaign.name,
            //                EndDate = Campaign.endDate,
            //                Sum = Campaign.sum,
            //                Themes = themes,
            //                Tags = tag,
            //                CampaignLinkImage = Campaign.image,
            //                CampaignLinkVideo = Campaign.video
            //            });
            //        }
            //    }
            //}
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> EditCampaign(TShitsViewModel model)
        {
            //TShirt shirt = _db.shirts.FirstOrDefault(item => item.id == model.id);
            //if (_db.TagsToCampaigns.FirstOrDefault(item => item.campaign == Campaign) != null)
            //    _db.TagsToCampaigns.RemoveRange(_db.TagsToCampaigns.Where(item => item.campaign == Campaign));
            //await _db.SaveChangesAsync();
            //shirt.name = model.TShirtName;
            //shirt.description = model.description;
            //shirt.theme = _db.themes.FirstOrDefault(item => item.name == model.theme);
            //shirt.image = model.image;

            //if (model.Tags != null)
            //{
            //    model.Tags = model.Tags.Replace("  ", " ");
            //    string[] tags = model.Tags.Split(" ");
            //    _db.Campaigns.Update(Campaign);
            //    for (int i = 0; i < tags.Length; i++)
            //    {
            //        if (tags[i] != "")
            //        {
            //            Tag tag = _db.Tag.FirstOrDefault(item => item.name == tags[i]);
            //            if (tag == null)
            //            {
            //                _db.Tag.Add(new Tag { name = tags[i] });
            //                await _db.SaveChangesAsync();
            //                _db.TagsToCampaigns.Add(new TagsToCampaign { campaign = Campaign, tag = _db.Tag.FirstOrDefault(item => item.name == tags[i]) });
            //            }
            //            else
            //            {
            //                _db.TagsToCampaigns.Add(new TagsToCampaign { campaign = Campaign, tag = tag });
            //            }
            //        }
            //    }
            //}
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        

    }
}