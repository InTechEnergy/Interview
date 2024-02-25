using ExampleApp.Api.Domain.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Domain.Academia.EntityConfiguration;

internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        _ = builder.HasKey(x => x.Id);
        _ = builder.HasIndex(s => s.Badge).IsUnique();
    }
}
