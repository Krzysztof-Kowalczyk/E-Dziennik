using System.Data.Entity;
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
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Subject> Subjects { get; set; }

    }
}