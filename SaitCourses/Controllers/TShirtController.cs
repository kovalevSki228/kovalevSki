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
using Hanssens.Net;

namespace SaitCourses.Controllers
{
    public class TShirtController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IConfiguration Configuration;
        private readonly UserManager<User> _userManager;        
        public TShirtController(ApplicationContext db, UserManager<User> userManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            Configuration = configuration;
        }
        private void deleteShirt(int id)
        {
            var rating = _db.ratings.FirstOrDefault(item => item.shirtid == id);
            var tshirt =  _db.tshirts.Find(id);
            if (rating != null)
            {
                _db.ratings.Remove(rating);
                _db.SaveChanges();
            }
            if (tshirt != null)
            {
                _db.tshirts.Remove(tshirt);
                _db.SaveChanges();
            }
        }
        private void CreateTopicAddDb(string name)
        {
            var topic = _db.topics.FirstOrDefault(item => item.nameTopic == name);
            if (topic == null)
            {
                _db.topics.Add(new Topic { nameTopic = name });
                _db.SaveChanges();
            }
        }
        private string GetTagsEditConstructor(int id)
        {
            var TagInShirtId = _db.tagInTShirts.Where(item => item.shirtid == id).ToArray();
            string tags = "";
            for (int i = 0; i < TagInShirtId.Length; i++)
                tags += " "+ _db.tags.FirstOrDefault(item => item.id == TagInShirtId[i].tagid).name.ToString();
            return tags;
        }
        private TShitsViewModel ShowConstructor(int id, string returnUrl, string userId)
        {
            string[] topics = _db.topics.Select(item => item.nameTopic).ToArray();

            if (id == 0)
            {
                return new TShitsViewModel
                {
                    returnUrl = returnUrl,
                    userid = userId,
                    Topics = topics,
                    Tegs = _db.tags.Select(item => item.name).ToArray(),
                    tag = _db.tags.ToArray()
                };
            }
            else
            {
                Shirt shirt =  _db.tshirts.Find(id);
                return new TShitsViewModel
                {
                    userid = userId,
                    returnUrl = returnUrl,
                    id = shirt.id,
                    image = shirt.image,
                    TShirtName = shirt.name,
                    description = shirt.description,
                    Topics = topics,
                    Tag = GetTagsEditConstructor(id),
                };
            }
        }
        private void ChangingParametersShirt(TShitsViewModel model, User user)
        {
            var result = _db.tshirts.FirstOrDefault(item => item.id == model.id);
            var topics = _db.topics.FirstOrDefault(item => item.nameTopic == model.Topic);
            result.name = model.TShirtName;
            result.description = model.description;
            result.themeId = topics.id;
            result.createDate = DateTime.Now.ToString("MM/dd/yyyy");
            result.Sex = model.sex;
            _db.tshirts.Update(result);
            _db.SaveChanges();
        }
        private string[] GetTegsEdit(TShitsViewModel model)
        {
            string tag = model.Tag.Replace("  ", " ");
            string[] tags = tag.Split(' ');
            return tags;
        }
        private void AddTagsEdit(TShitsViewModel model)
        {
            string[] tags = GetTegsEdit(model);
            for (int i = 0; i < tags.Length; i++)
            {
                if (_db.tags.FirstOrDefault(item => item.name == tags[i]) == null)
                {
                    _db.tags.Add(new Tag { name = tags[i] });
                    _db.SaveChanges();
                }
                Shirt shirt = _db.tshirts.FirstOrDefault(item => item.name == model.TShirtName);
                Tag tag1 = _db.tags.FirstOrDefault(item => item.name == tags[i]);
                _db.tagInTShirts.Add(new TagInTShirt
                {
                    shirtid = shirt.id,
                    tagid = tag1.id
                });
                _db.SaveChanges();
            }
        }
        private BasketViewModel GetShoppingList(User user)
        {
            var _basket = _db.baskets.Where(item => item.userId == user.Id).ToList();
            return new BasketViewModel
            {
                userId = user.Id,
                basket = _basket.ToList()
            };
        }
        private void AddBasket(BasketViewModel model, User user, int id)
        {
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
            _db.SaveChanges();
        }
        private async Task<User> GetUser()
        {
            User user = await _userManager.GetUserAsync(User);
            return user;
        }
        private async void BayAuthenticated(User user)
        {
            var result = _db.baskets.Where(item => item.userId == user.Id 
                && item.purchaseStatus == false).ToList();
            string message = "";
            foreach (var status in result)
            {
                status.purchaseStatus = true;
                message += "\n" + status.nameShirt.ToString() + ", price 30 $";
                _db.baskets.Update(status);
                _db.SaveChanges();
            }
            EmailService emailservice = new EmailService(Configuration);
            await emailservice.SendEmailAsync(user.Email, "Purchase",
                "you bought t-shirts: " + message);
        }
        private async void BayNoAuthenticated(BasketViewModel model)
        {
            string message = model.noAutorize;
            EmailService emailservice = new EmailService(Configuration);
            await emailservice.SendEmailAsync(model.Email, "Purchase",
                "you bought t-shirts: " + message);
        }
        private string UserVerificationAndPurchase(BasketViewModel model, User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                BayAuthenticated(user);
                return "Bay";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    BayNoAuthenticated(model);
                    return "Bay";
                }
                else
                    return "Basket";
            }
        }

        [TypeFilter(typeof(UserFilters))]
        public IActionResult Delete(int id, string returnUrl)
        {
            deleteShirt(id);
            return Redirect(returnUrl);
        }

        public IActionResult CreateTopic() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateTopic(string name)
        {

            CreateTopicAddDb(name);
            return RedirectToAction("Index", "Home");
        }

        [TypeFilter(typeof(UserFilters))]
        public IActionResult Constructor(int id, string returnUrl, string userId)
        {
            return View(ShowConstructor(id, returnUrl, userId));
        }

        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Edit(TShitsViewModel model, string returnUrl)
        {      
            ChangingParametersShirt(model, await GetUser());
            if(!String.IsNullOrEmpty(model.Tag))
               AddTagsEdit(model);
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Stores()
        {
            return View(GetShoppingList(await GetUser())) ;
        }
        [HttpGet]
        public async Task<IActionResult> basket()
        {
            if(User.Identity.IsAuthenticated)
                return View(GetShoppingList(await GetUser()));
            else
                return View("Basket");
        }
        
        [HttpPost]
        public async Task<IActionResult> basket(int id, BasketViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                AddBasket(model, await GetUser(), id);
                return View(GetShoppingList(await GetUser()));
            }
            else
                return View("Basket");
        }
        
        [HttpPost]
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var result = _db.baskets.FirstOrDefault(item => item.id == id);
            _db.baskets.Remove(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("basket");
        }

        [HttpPost]
        public async Task<IActionResult> Bay(BasketViewModel model, string messageBasket)
        {
            return View(UserVerificationAndPurchase(model, await GetUser()));
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