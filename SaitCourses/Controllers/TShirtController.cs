using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitCourses.Models;
using SaitCourses.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace SaitCourses.Controllers
{
    public class TShirtController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;
        public IActionResult Index()
        {
            return View();
        }

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
            TShirt shirt = _db.shirts.FirstOrDefault(item => item.id == model.id);
            //if (_db.TagsToCampaigns.FirstOrDefault(item => item.campaign == Campaign) != null)
            //    _db.TagsToCampaigns.RemoveRange(_db.TagsToCampaigns.Where(item => item.campaign == Campaign));
            await _db.SaveChangesAsync();
            shirt.name = model.TShirtName;
            shirt.description = model.description;
            shirt.theme = _db.themes.FirstOrDefault(item => item.name == model.theme);
            shirt.image = model.image;

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