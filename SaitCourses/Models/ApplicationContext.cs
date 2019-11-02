using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SaitCourses.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
       // public DbSet<TShirt> shirts { get; set; }
        //public DbSet<Comment> comments { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<Theme> themes { get; set; }
        public DbSet<Shirt> tshirts { get; set;}
        public DbSet<Rating> ratings { get; set; }
        public DbSet<Topic> topics { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<Basket> baskets { get; set; }
        public DbSet<TagInTShirt> tagInTShirts { get; set; }
        public DbSet<Achievement> achievements { get; set; }
        public DbSet<AchievementsUsers> achievementsUsers { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
