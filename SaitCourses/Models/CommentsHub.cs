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
        public async Task Send(string message, string userName)
        {

            await Clients.All.SendAsync("Send", message, userName);
        }
        
    }
}
