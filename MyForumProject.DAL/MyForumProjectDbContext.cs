using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyForumProject.BL.Entities;

namespace MyForumProject.DAL
{
    public class MyForumProjectDbContext : IdentityDbContext<User>
    {
        public DbSet<Blog>? Blogs { get; set; }
        public DbSet<Post>? Posts { get; set; }
        
        public override DbSet<User>? Users { get; set; }
        public MyForumProjectDbContext(DbContextOptions<MyForumProjectDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /* TODO: add code*/
        }
    }
}
