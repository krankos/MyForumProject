using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForumProject.DAL
{
    public class DataContextFactory : IDesignTimeDbContextFactory<MyForumProjectDbContext>
    {
        public MyForumProjectDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyForumProjectDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyForumProjectDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new MyForumProjectDbContext(optionsBuilder.Options);
        }
    }
}
