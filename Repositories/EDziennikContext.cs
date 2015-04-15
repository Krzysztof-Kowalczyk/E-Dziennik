using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Models.Models;

namespace Repositories
{
    public class EDziennikContext : DbContext
    {
        public EDziennikContext()
            : base("DefaultConnection")
        { }

        public DbSet<Mark> Marks { get; set; }
        public DbSet<Student> Students  { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Classs> Classes { get; set; }

    }
}