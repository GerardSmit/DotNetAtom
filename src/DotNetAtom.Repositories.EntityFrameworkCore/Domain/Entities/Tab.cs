using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Tab : ITimestamp
{
    public int Id { get; set; }

    public int TabOrder { get; set; }

    public int? PortalId { get; set; }

    public Portal? Portal { get; set; }

    public string TabName { get; set; }

    public bool IsVisible { get; set; }

    public int? ParentId { get; set; }

    public string? IconFile { get; set; }

    public bool DisableLink { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? KeyWords { get; set; }

    public bool IsDeleted { get; set; }

    public string? Url { get; set; }

    public string? SkinSrc { get; set; }

    public string? ContainerSrc { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? RefreshInterval { get; set; }

    public string? PageHeadText { get; set; }

    public bool IsSecure { get; set; }

    public bool PermanentRedirect { get; set; }

    public double SiteMapPriority { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? IconFileLarge { get; set; }

    public string? CultureCode { get; set; }

    public int? ContentItemId { get; set; }

    public Guid UniqueId { get; set; }

    public Guid VersionGuid { get; set; }

    public Guid? DefaultLanguageGuid { get; set; }

    public Guid LocalizedVersionGuid { get; set; }

    public int Level { get; set; }

    public string TabPath { get; set; }

    public ICollection<TabModule> TabModules { get; set; } = new List<TabModule>();

    public ICollection<TabSetting> TabSettings { get; set; } = new List<TabSetting>();

    [NotMapped]
    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();

    public bool TryGetString(string key, [NotNullWhen(true)] out string? str)
    {
        var setting = TabSettings.FirstOrDefault(s => s.SettingName == key);

        if (setting == null)
        {
            str = null;
            return false;
        }

        str = setting.SettingValue;
        return true;
    }
}

public class TabTypeConfiguration : IEntityTypeConfiguration<Tab>
{
    public void Configure(EntityTypeBuilder<Tab> builder)
    {
        builder.ToTable("Tabs");
        builder.HasKey(pa => pa.Id);

        builder.Property(t => t.Id)
            .HasColumnName("TabID");

        builder.Property(t => t.PortalId)
            .HasColumnName("PortalID")
            .IsRequired(false);

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(t => t.Portal)
            .WithMany(p => p.Tabs)
            .HasForeignKey(t => t.PortalId)
            .HasPrincipalKey(p => p.Id);
    }
}
