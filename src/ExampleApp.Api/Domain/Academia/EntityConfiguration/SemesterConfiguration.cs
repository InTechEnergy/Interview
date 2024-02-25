using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Domain.Academia.EntityConfiguration;

internal class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        _ = builder.HasKey(x => x.Id);
        _ = builder.Property(x => x.Start).HasColumnName("StartDate");
        _ = builder.Property(x => x.End).HasColumnName("EndDate");
    }
}
