using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Package : IEntity, ITimestamp
{
    public int Id { get; set; }

    public int? PortalId { get; set; }

    public required string Name { get; set; }

    public required string FriendlyName { get; set; }

    public string? Description { get; set; }

    public required string PackageTypeCode { get; set; }

    public required string Version { get; set; }

    public string? License { get; set; }

    public string? Owner { get; set; }

    public string? Organization { get; set; }

    public string? Url { get; set; }

    public string? Email { get; set; }

    public string? ReleaseNotes { get; set; }

    public bool IsSystemPackage { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? FolderName { get; set; }

    public string? IconFile { get; set; }

    public PackageType PackageType { get; set; } = default!;

    public ICollection<DesktopModule> DesktopModules { get; set; } = new List<DesktopModule>();
}

public class PackageTypeConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("Packages");
        builder.HasKey(pa => pa.Id);

        builder.Property(p => p.Id)
            .HasColumnName("PackageID");

        builder.Property(p => p.PortalId)
            .HasColumnName("PortalID");

        builder.Property(p => p.Name)
            .HasMaxLength(128);

        builder.Property(p => p.FriendlyName)
            .HasMaxLength(250);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.Version)
            .HasMaxLength(50);

        builder.Property(p => p.FolderName)
            .HasMaxLength(128);

        builder.Property(p => p.IconFile)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(p => p.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.Property(p => p.PackageTypeCode)
            .HasColumnName("PackageType")
            .HasMaxLength(100);

        builder.HasOne(p => p.PackageType)
            .WithMany()
            .HasForeignKey(p => p.PackageTypeCode)
            .HasPrincipalKey(r => r.Code);
    }
}
