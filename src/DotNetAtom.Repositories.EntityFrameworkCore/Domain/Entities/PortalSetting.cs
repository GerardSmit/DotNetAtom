using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class PortalSetting : ITimestamp
{
    public int PortalSettingId { get; set; }

    public int PortalId { get; set; }

    public Portal Portal { get; set; } = null!;

    public string SettingName { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? CultureCode { get; set; }

    public bool IsSecure { get; set; }
}

public class PortalSettingTypeConfiguration : IEntityTypeConfiguration<PortalSetting>
{
    public void Configure(EntityTypeBuilder<PortalSetting> builder)
    {
        builder.ToTable("PortalSettings");
        builder.HasKey(ps => ps.PortalSettingId);

        builder.Property(ps => ps.PortalSettingId)
            .HasColumnName("PortalSettingID");

        builder.Property(ps => ps.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(ps => ps.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(pa => pa.Portal)
            .WithMany(p => p.Settings)
            .HasForeignKey(pa => pa.PortalId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();
    }
}
