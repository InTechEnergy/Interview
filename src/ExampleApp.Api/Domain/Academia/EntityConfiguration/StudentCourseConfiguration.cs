using ExampleApp.Api.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Domain.Academia.EntityConfiguration;

internal class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        _ = builder.HasIndex(sc => new { sc.StudentId, sc.CourseId }).IsUnique();
    }
}
