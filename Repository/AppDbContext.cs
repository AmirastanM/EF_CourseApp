using Domain.Models;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = Domain.Models.Group;

namespace Repository
{
    internal class AppDbContext : DbContext
    {

        public DbSet<Education> Educations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=amirastan-pc\\sqlexpress;Initial Catalog=EF_CourseApp;Integrated Security=True");

        }
    }

}
