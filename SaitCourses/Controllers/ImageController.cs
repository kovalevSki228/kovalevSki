using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.ViewModels;
using SaitCourses.Models;
using Microsoft.AspNetCore.Identity;
using SaitCourses.Filters;
namespace SaitCourses.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationContext _db;

        private readonly UserManager<User> _userManager;

        public ImageController(ApplicationContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        private void AddDbImage(TShitsViewModel model)
        {

            _db.images.Add(new Image
            {
                image = model.image
            });
            _db.SaveChanges();

        }
        private string[] GetTegsConstructor(TShitsViewModel model)
        {
            string tag = model.Tag.Replace("  ", " ");
            string[] tags = tag.Split(' ');
            return tags;
        }
        private Topic GetTopicConstructor(TShitsViewModel model)
        {
            return _db.topics.FirstOrDefault(item => item.nameTopic == model.Topic);
        }
        private void AddTag(string[] tags, TShitsViewModel model)
        {
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
        private string description(TShitsViewModel model)
        {
            if (model.description != "" && !String.IsNullOrEmpty(model.description))
            { return model.description; }
            else
            { return "There should have been text"; }
        }
        private string name(TShitsViewModel model)
        {
            if (model.TShirtName != "" && !String.IsNullOrEmpty(model.TShirtName))
            { return model.TShirtName; }
            else
            { return "There should have been text"; }
        }
        private void AddShirt(TShitsViewModel model, Topic topics, User user, string description, string name)
        {
            _db.tshirts.Add(new Shirt
            {
                image = model.image,
                name = name,
                description = description,
                userId = user.Id,
                themeId = topics.id,
                createDate = DateTime.Now.ToString("MM/dd/yyyy"),
                Sex = model.sex
            }); ;
            _db.SaveChanges();
        }
        private async Task<User> GetUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id); ;
            return user;
        }

        //[TypeFilter(typeof(UserFilters))]
        //public async Task<IActionResult> CreateShirt()
        //{
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction("Index", "Users");
        //}
        [TypeFilter(typeof(UserFilters))]
        [HttpPost]
        public IActionResult AddImage(TShitsViewModel model)
        {
            AddDbImage(model);
            return RedirectToAction("Index", "Home");
        }
        [TypeFilter(typeof(UserFilters))]
        public async Task<IActionResult> Constructor(TShitsViewModel model, string userId, string returnUrl)
        {
            var topics = GetTopicConstructor(model);
            User user = await _userManager.FindByIdAsync(userId);            

            _db.tshirts.Add(new Shirt
            {
                image = model.image,
                name = name(model),
                description = description(model),
                userId = user.Id,
                themeId = topics.id,
                createDate = DateTime.Now.ToString("MM/dd/yyyy"),
                Sex = model.sex
            }); ;
            //}
            await _db.SaveChangesAsync();
            if(!String.IsNullOrEmpty(model.Tag))
                AddTag(GetTegsConstructor(model), model);
            return Redirect(returnUrl);
        }
        //[TypeFilter(typeof(UserFilters))]
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}