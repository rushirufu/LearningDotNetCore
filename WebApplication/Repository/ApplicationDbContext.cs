using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // public DbSet<MyIdentityModel> Product { get; set; }
    }
}

public class SchoolDBContext : DbContext
{
    public SchoolDBContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //use this to configure the contex
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
    //entities
    //public DbSet<Student> Students { get; set; }
   // public DbSet<Course> Courses { get; set; }
}