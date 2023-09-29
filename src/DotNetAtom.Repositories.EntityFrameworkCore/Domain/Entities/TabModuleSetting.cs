using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class TabModuleSetting : ITimestamp
{
    public int TabModuleId { get; set; }

    public TabModule TabModule { get; set; }

    public string SettingName { get; set; }

    public string SettingValue { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}

public class TabModuleSettingTypeConfiguration : IEntityTypeConfiguration<TabModuleSetting>
{
    public void Configure(EntityTypeBuilder<TabModuleSetting> builder)
    {
        builder.ToTable("TabModuleSettings");
        builder.HasKey(tms => new { tms.TabModuleId, tms.SettingName });

        builder.Property(tms => tms.TabModuleId)
            .HasColumnName("TabModuleID");

        builder.Property(tms => tms.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(tms => tms.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(tms => tms.TabModule)
            .WithMany(tm => tm.ModuleSettings)
            .HasForeignKey(tms => tms.TabModuleId)
            .HasPrincipalKey(tm => tm.Id)
            .IsRequired();
    }
}
