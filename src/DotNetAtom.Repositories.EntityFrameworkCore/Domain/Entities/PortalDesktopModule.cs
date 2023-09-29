using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class PortalDesktopModule : IEntity
{
    public int Id { get; set; }

    public int PortalId { get; set; }

    public Portal Portal { get; set; }

    public int DesktopModuleId { get; set; }

    public DesktopModule DesktopModule { get; set; }
}

public class PortalDesktopModuleTypeConfiguration : IEntityTypeConfiguration<PortalDesktopModule>
{
    public void Configure(EntityTypeBuilder<PortalDesktopModule> builder)
    {
        builder.ToTable("PortalDesktopModules");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("PortalDesktopModuleID");

        builder.Property(e => e.PortalId)
            .HasColumnType("PortalID");

        builder.Property(e => e.DesktopModuleId)
            .HasColumnType("DesktopModuleID");

        builder.HasOne(e => e.Portal)
            .WithMany(e => e.PortalDesktopModule)
            .HasForeignKey(e => e.PortalId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired();

        builder.HasOne(e => e.DesktopModule)
            .WithMany(e => e.PortalDesktopModule)
            .HasForeignKey(e => e.PortalId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired();
    }
}
