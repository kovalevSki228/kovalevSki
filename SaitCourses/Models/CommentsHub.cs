using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SaitCourses.Models
{
    public class CommentsHub : Hub
    {
        private ApplicationContext _db;
        public CommentsHub(ApplicationContext context)
        {
            _db = context;
        }
        public async Task Send(string message, string userName, string tShirt)
        {
            User user = _db.Users.FirstOrDefault(item => item.UserName == userName);
            Comment comment = _db.comments.FirstOrDefault(item => item.tShirtId + "" == tShirt && item.user == user);
            if (comment == null)
            {
                if (message != "")
                {
                    _db.comments.Add(new Comment { user = user,userId = user.Id,  tShirtId = Convert.ToInt32(tShirt), text = message });
                 //   await _db.SaveChangesAsync();
                }
            }
            else
            {
                if (message == "")
                {
                    _db.comments.Remove(comment);
                    _db.SaveChanges();
                }
                else
                {
                    comment.text = message;
                    _db.comments.Update(comment);
                    _db.SaveChanges();
                }
            }
            comment = _db.comments.FirstOrDefault(item => item.tShirtId + "" == tShirt && item.user == user);
            if (comment != null)
                await Clients.All.SendAsync("Send", message, userName, comment.id);
            else
                await Clients.All.SendAsync("Send", message, userName, 0);
        }
        public async Task Rating(string userName, bool value, string tShirtId)
        {
            int changeValue = 0;
            Comment comment = _db.comments.FirstOrDefault(item => item.id + "" == tShirtId);
            User user = _db.Users.FirstOrDefault(item => item.UserName == userName);
            Like rating = _db.likes.FirstOrDefault(item => item.shirtId == Int32.Parse(tShirtId) && item.userId == user.Id);
            int rat;
            if(value)
            {
                rat = 1;
            }
            else
            {
                rat = -1;
            }
            if (rating == null)
            {
                _db.likes.Add(new Like { userId = user.Id, shirtId = Int32.Parse(tShirtId), like = rat });
                _db.SaveChanges();
                if (value) changeValue = 1;
                else changeValue = -1;
            }
            else
            {
                if (rating.like == 1)
                {
                    _db.likes.Remove(rating);
                    _db.SaveChanges();
                    if (value) changeValue = -1;
                    else changeValue = 1;
                }
                else
                {
                    rating.like = 1;
                    _db.likes.Update(rating);
                    _db.SaveChanges();
                    if (value) changeValue = 2;
                    else changeValue = -2;
                }
            }
            await Clients.All.SendAsync("Rating", tShirtId, changeValue);
        }
    }
}
