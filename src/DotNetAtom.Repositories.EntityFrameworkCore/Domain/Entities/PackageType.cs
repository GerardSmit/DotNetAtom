using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class PackageType
{
    public required string Code { get; set; }

    public required string Description { get; set; }

    public required SecurityAccessLevel SecurityAccessLevel { get; set; }

    public string? EditorControlSrc { get; set; }
}

public class PackageTypeTypeConfiguration : IEntityTypeConfiguration<PackageType>
{
    public void Configure(EntityTypeBuilder<PackageType> builder)
    {
        builder.ToTable("PackageTypes");
        builder.HasKey(pa => pa.Code);

        builder.Property(p => p.Code)
            .HasColumnName("PackageType")
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.EditorControlSrc)
            .HasMaxLength(250);
    }
}
