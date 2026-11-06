using CourseManagemenbt.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagemenbt.Data;

public class CourseDbContext(DbContextOptions<CourseDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(course => course.Title).IsRequired().HasMaxLength(200);
            entity.Property(course => course.Description).HasMaxLength(2000);
            entity.Property(course => course.Instructor).IsRequired().HasMaxLength(200);
        });
    }
}
