using ExampleApp.Api.Domain.Academia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Infrastructure.Configurations.Academia;

internal sealed class SemesterConfiguration :  IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.ToTable("Semesters");

        builder.HasKey(s => s.Id);

        builder
            .Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(s => s.End)
            .HasColumnName("EndDate");

        builder
            .Property(s => s.Start)
            .HasColumnName("StartDate");
    }
}
