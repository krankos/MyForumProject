using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyForumProject.BL.Entities;
using Microsoft.AspNetCore.Identity;

namespace MyForumProject.DAL
{
    public class MyForumProjectDbContext : IdentityDbContext<User>
    {
        public DbSet<Blog>? Blogs { get; set; }
        public DbSet<Post>? Posts { get; set; }
		public DbSet<Comment>? Comments { get; set; }

		public override DbSet<User>? Users { get; set; }
        public MyForumProjectDbContext(DbContextOptions<MyForumProjectDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /* TODO: add code */

            modelBuilder.Entity<Blog>()
                    .HasMany<Post>(g => g.Posts)
                    .WithOne(s => s.Blog)
                    .HasForeignKey(s => s.BlogId);


            //onetomany relationship between post and comment
            modelBuilder.Entity<Post>()
                .HasMany<Comment>(g => g.Comments)
                .WithOne(s => s.Post)
                .HasForeignKey(s => s.PostId);

            //one to many relationship between user and blog
            modelBuilder.Entity<User>()
                .HasMany<Blog>(g => g.Blogs)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId);


            //one to many relationship between user and post
            modelBuilder.Entity<User>()
                .HasMany<Post>(g => g.Posts)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId);


            //one to many relationship between user and comment
            modelBuilder.Entity<User>()
                .HasMany<Comment>(g => g.Comments)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId);
				


			modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
