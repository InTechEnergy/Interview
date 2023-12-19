using Azure;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace ExampleApp.Api.Domain.Academia;

internal class AcademiaDbContext : DbContext
{
    public AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : base(options)
    {
    }

    internal DbSet<Course> Courses { get; set; }
    internal DbSet<Professor> Professors { get; set; }
    internal DbSet<Semester> Semesters { get; set; }
    internal DbSet<Student> Students { get; set; }
    internal DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Semester>(
            e =>
            {
                e.Property(x => x.Start).HasColumnName("StartDate");
                e.Property(x => x.End).HasColumnName("EndDate");
            });

        builder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        builder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        builder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
