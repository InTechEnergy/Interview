using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Infrastructure.Configurations.Academia;

internal sealed class CourseConfiguration :  IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(c => c.Semester);

        builder.HasOne(c => c.Professor);

        builder.Property(c => c.Description);

        builder.Property(c => c.CreatedOn);

        builder.Property(c => c.LastModifiedOn);
    }
}
