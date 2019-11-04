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
        public async Task Send(string message, string userName, int shirtId)
        {
            User user = _db.Users.FirstOrDefault(item => item.UserName == userName);
            Shirt shirt = _db.tshirts.FirstOrDefault(i => i.id == shirtId);
            if (message != "")
            {
                _db.comments.Add(new Comment { user = user, tShirt = shirt , text = message, like = 0 });
                _db.SaveChanges();
            }
            
            var comment = _db.comments.Where(item => item.tShirtId == shirtId && item.user == user).ToArray();
            if (comment!= null)
                await Clients.All.SendAsync("Send", message, userName, comment[comment.Length - 1].id);
            else
                await Clients.All.SendAsync("Send", message, userName, 0);
        }
        public async Task Rating(string userName, int value, int commentId)
        {
            int changeValue = 0;
            Comment comment = _db.comments.FirstOrDefault(item => item.id == commentId);
            User user = _db.Users.FirstOrDefault(item => item.UserName == userName);
            Like like = _db.likes.FirstOrDefault(item => item.shirtId == comment.id && item.userId == user.Id);
            if (like == null)
            {
                _db.likes.Add(new Like { userId = user.Id, like = value, shirtId = commentId });
                _db.SaveChanges();
                changeValue = value;
            }
            else
            {
                if (like.like == value)
                {
                    _db.likes.Remove(like);
                    _db.SaveChanges();
                     changeValue = value*-1;
                }
                else
                {
                    like.like = value;
                    _db.likes.Update(like);
                    _db.SaveChanges();
                    if (value == 1) changeValue = 2;
                    else changeValue = -2;
                }
            }
            comment.like += changeValue;
            _db.comments.Update(comment);
            _db.SaveChanges();
            await Clients.All.SendAsync("Rating", commentId, changeValue);
        }

    }
   
}
