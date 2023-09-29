using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class PortalLocalization : ITimestamp
{
    public int PortalId { get; set; }

    public Portal Portal { get; set; } = default!;
        
    public required string CultureCode { get; set; }

    public required string PortalName { get; set; }

    public string? LogoFile { get; set; }

    public string? FooterText { get; set; }

    public string? Description { get; set; }

    public string? KeyWords { get; set; }

    public string? BackgroundFile { get; set; }

    public int? HomeTabId { get; set; }

    public int? LoginTabId { get; set; }

    public int? UserTabId { get; set; }

    public int? AdminTabId { get; set; }

    public int? SplashTabId { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public int? RegisterTabId { get; set; }

    public int? SearchTabId { get; set; }
}

public class PortalLocalizationTypeConfiguration : IEntityTypeConfiguration<PortalLocalization>
{
    public void Configure(EntityTypeBuilder<PortalLocalization> builder)
    {
        builder.ToTable("PortalLocalization");
        builder.HasKey(pl => new { pl.PortalId, pl.CultureCode });

        builder.Property(pl => pl.PortalId)
            .HasColumnName("PortalID");

        builder.Property(pl => pl.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(pl => pl.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(pl => pl.Portal)
            .WithMany(p => p.Localizations)
            .HasForeignKey(pl => pl.PortalId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();
    }
}
