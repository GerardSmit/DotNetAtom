using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class DesktopModule : IEntity, ITimestamp
{
    public int Id { get; set; }

    public string FriendlyName { get; set; }

    public string? Description { get; set; }

    public string? Version { get; set; }

    public bool IsPremium { get; set; }

    public bool IsAdmin { get; set; }

    public string? BusinessControllerClass { get; set; }

    public string FolderName { get; set; }

    public string ModuleName { get; set; }

    public int SupportedFeatures { get; set; }

    public string? CompatibleVersions { get; set; }

    public string? Dependencies { get; set; }

    public string? Permissions { get; set; }

    public int PackageId { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public int ContentItemId { get; set; }

    public Package Package { get; set; } = default!;

    public ICollection<ModuleDefinition> ModuleDefinitions { get; set; } = new List<ModuleDefinition>();

    public ICollection<PortalDesktopModule> PortalDesktopModule { get; set; } = new List<PortalDesktopModule>();
}

public class DesktopModuleTypeConfiguration : IEntityTypeConfiguration<DesktopModule>
{
    public void Configure(EntityTypeBuilder<DesktopModule> builder)
    {
        builder.ToTable("DesktopModules");
        builder.HasKey(m => m.Id);

        builder.Property(t => t.Id)
            .HasColumnName("DesktopModuleID");

        builder.Property(t => t.PackageId)
            .HasColumnName("PackageID");

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(m => m.Package)
            .WithMany(p => p.DesktopModules)
            .HasForeignKey(pa => pa.PackageId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();
    }
}
