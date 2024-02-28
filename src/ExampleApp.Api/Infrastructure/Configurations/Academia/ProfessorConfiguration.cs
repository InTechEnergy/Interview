using ExampleApp.Api.Domain.Academia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Api.Infrastructure.Configurations.Academia;

internal sealed class ProfessorConfiguration :  IEntityTypeConfiguration<Lecturer>
{
    public void Configure(EntityTypeBuilder<Lecturer> builder)
    {
        builder.ToTable("Professors");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.FullName)
            .IsRequired();

        builder
            .Property(p => p.Extension);
    }
}
