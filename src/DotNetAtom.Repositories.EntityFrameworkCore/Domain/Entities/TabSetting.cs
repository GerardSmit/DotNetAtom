using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class TabSetting : ITimestamp
{
    public int TabId { get; set; }

    public string SettingName { get; set; } = default!;

    public string SettingValue { get; set; } = default!;

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public Tab Tab { get; set; } = default!;
}

public class TabSettingTypeConfiguration : IEntityTypeConfiguration<TabSetting>
{
    public void Configure(EntityTypeBuilder<TabSetting> builder)
    {
        builder.ToTable("TabSettings");
        builder.HasKey(ts => new { ts.TabId, ts.SettingName });

        builder.Property(ts => ts.TabId)
            .HasColumnName("TabID");

        builder.Property(ts => ts.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(ts => ts.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(t => t.Tab)
            .WithMany(ts => ts.TabSettings)
            .HasForeignKey(t => t.TabId)
            .HasPrincipalKey(ts => ts.Id)
            .IsRequired();
    }
}
