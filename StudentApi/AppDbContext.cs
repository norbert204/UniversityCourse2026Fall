using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();

        public DbSet<Lesson> Lessons => Set<Lesson>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(student => student.NeptunCode);
            modelBuilder.Entity<Lesson>()
                .HasMany(lesson => lesson.Students)
                .WithMany(student => student.Lessons)
                .UsingEntity("StudentLessons");
        }
    }
}
