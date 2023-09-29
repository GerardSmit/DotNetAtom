using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class HostSetting : ITimestamp
{
    public string Name { get; set; }

    public string Value { get; set; }

    public bool SettingIsSecure { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}

public class HostSettingTypeConfiguration : IEntityTypeConfiguration<HostSetting>
{
    public void Configure(EntityTypeBuilder<HostSetting> builder)
    {
        builder.ToTable("HostSettings");
        builder.HasKey(gs => gs.Name);

        builder.Property(t => t.Name)
            .HasColumnName("SettingName");

        builder.Property(t => t.Value)
            .HasColumnName("SettingValue");

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");
    }
}
