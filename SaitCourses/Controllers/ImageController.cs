using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.ViewModels;
using SaitCourses.Models;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IActionResult> CreateShirt()
        {
            //    User user = await _userManager.GetUserAsync(User);
              //  _db.shirts.Add(new TShirt
              //  {
              ////      user = user,
              //      name = "Tshirt =)",
              //      createDate = DateTime.Now.ToString("MM/dd/yyyy"),
              //  });
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(TShitsViewModel model)
        {
            //TShirt shirt = _db.shirts.FirstOrDefault(item => item.id == model.id);
            //if(shirt != null)
            //{
                _db.images.Add(new Image {
               //     tShirt = shirt,
                    image = model.image
                });
            //}
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Constructor(TShitsViewModel model)
        {
            //TShirt shirt = _db.shirts.FirstOrDefault(item => item.id == model.id);
            //if(shirt != null)
            //{
            var topics = _db.topics.FirstOrDefault(item => item.nameTopic == model.Topic);
            User user = await _userManager.GetUserAsync(User);
            _db.tshirts.Add(new Shirt
            {
                //     tShirt = shirt,
                image = model.image,
                name = model.TShirtName,
                description = model.description,
                userId = user.Id,
                themeId = topics.id
                //users = user
            });; 
            //}
            await _db.SaveChangesAsync();

            return RedirectToAction("Constructor", "Users");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}