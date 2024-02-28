using ExampleApp.Api.Domain.Academia.Contracts;
using ExampleApp.Api.Domain.SharedKernel.Contracts;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using ExampleApp.Api.Domain.Students.Entities;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia;

internal class AcademiaDbContext : DbContext, IDbContext
{
    public AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : base(options)
    {
    }

    internal DbSet<Course> Courses { get; set; }
    internal DbSet<Lecturer> Professors { get; set; }
    internal DbSet<Semester> Semesters { get; set; }
    internal DbSet<Student> Students { get; set; } 
    internal DbSet<StudentCourses> StudentCourses { get; set; }

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

    public Task<int> SaveChangesAsync()
    {
        OnBeforeSaveChanges();
        return base.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        OnBeforeSaveChanges();
        return base.SaveChanges();
    }

    private void OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var entityEntries = ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);
        foreach (var entry in entityEntries)
        {
            if (entry.Entity is IAuditable e)
            {
                if (entry.State == EntityState.Added)
                {
                    e.CreatedOn = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    e.IsDeleted = true;
                }

                e.ChangedOn = DateTime.UtcNow;
            }
        }
    }
}
