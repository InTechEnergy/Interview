using ExampleApp.Api.Domain.Academia.EntityConfiguration;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

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

        _ = builder.ApplyConfiguration(new SemesterConfiguration());
        _ = builder.ApplyConfiguration(new StudentConfiguration());
        _ = builder.ApplyConfiguration(new StudentCourseConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        _ = builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
