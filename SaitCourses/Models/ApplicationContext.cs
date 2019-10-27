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
        public DbSet<TShirt> shirts { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<Theme> themes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
