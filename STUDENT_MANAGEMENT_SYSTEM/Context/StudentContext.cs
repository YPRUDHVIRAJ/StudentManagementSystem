using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Context
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<YearOfJoining> YearsOfJoining { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        
    }
}
