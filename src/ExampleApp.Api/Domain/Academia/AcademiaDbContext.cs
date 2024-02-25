using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;
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

        _ = builder.Entity<Semester>(
            e =>
            {
                _ = e.Property(x => x.Start).HasColumnName("StartDate");
                _ = e.Property(x => x.End).HasColumnName("EndDate");
            });

        _ = builder.Entity<Student>()
            .HasIndex(s => s.Badge)
            .IsUnique();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        _ = builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
