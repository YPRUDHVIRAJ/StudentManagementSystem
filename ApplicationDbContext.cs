using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<YearOfJoining> YearsOfJoining { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CourseID);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.YearOfJoining)
                .WithMany(y => y.Students)
                .HasForeignKey(s => s.YearOfJoiningID);

            modelBuilder.Entity<Course>()
                 .HasOne(c => c.Department)
                 .WithMany(d => d.Courses)
                 .HasForeignKey(c => c.DepartmentID);

            modelBuilder.Entity<YearOfJoining>()
                 .Property(y => y.Year)
                 .IsRequired();
        }
    }
}
