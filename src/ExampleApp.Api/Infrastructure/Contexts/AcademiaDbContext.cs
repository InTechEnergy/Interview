using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia;

internal class AcademiaDbContext : DbContext
{
    public AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    internal DbSet<Course> Courses { get; set; }
    internal DbSet<Professor> Professors { get; set; }
    internal DbSet<Semester> Semesters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AcademiaDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
