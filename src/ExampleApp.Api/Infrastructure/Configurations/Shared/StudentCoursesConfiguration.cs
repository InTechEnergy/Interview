using ExampleApp.Api.Domain.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Infrastructure.Configurations.Shared;

internal sealed class StudentCoursesConfiguration : IEntityTypeConfiguration<StudentCourses>
{
    public void Configure(EntityTypeBuilder<StudentCourses> builder)
    {
        builder.ToTable("StudentCourses");

        builder.HasKey(sc => sc.Id);

        builder
            .Property(sc => sc.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(sc => sc.Student)
            .WithMany()
            .HasForeignKey(sc => sc.StudentId);

        builder
            .HasMany(sc => sc.Courses)
            .WithMany();
    }
}
