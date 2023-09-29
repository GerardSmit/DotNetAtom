using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class TabModule : ITimestamp
{
    private IDictionary<string, string>? _settings;
    private HtmlText? _htmlContent;
    private readonly IDictionary<int, bool>? _rolePermissions;

    /// <inheritdoc cref="IEntity.Id" />
    public int Id { get; set; }

    public int TabId { get; set; }

    public Tab Tab { get; set; }

    public int ModuleId { get; set; }

    public Module Module { get; set; }

    public string PaneName { get; set; }

    public int ModuleOrder { get; set; }

    [NotMapped]
    public HtmlText? CurrentHtmlText => Module.HtmlTexts
        .Where(ht => ht.IsPublished.HasValue && ht.IsPublished.Value)
        .MaxBy(ht => ht.Version ?? 0);

    [NotMapped]
    public string? HtmlContent => CurrentHtmlText != null
        ? WebUtility.HtmlDecode(CurrentHtmlText.Content)
        : null;

    [NotMapped] public DateTime? LastHtmlModifiedOnDate
    {
        get
        {
            var htmlText = CurrentHtmlText;

            if (htmlText == null)
            {
                return null;
            }

            return htmlText.LastModifiedOnDate ?? htmlText.CreatedOnDate;
        }
    }

    public int CacheTime { get; set; }

    public string? Alignment { get; set; }

    public string? Color { get; set; }

    public string? Border { get; set; }

    public string? IconFile { get; set; }

    public int Visibility { get; set; }

    public string? ContainerSrc { get; set; }

    public DateTime? WebSliceExpiryDate { get; set; }

    public int? WebSliceTtl { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public bool IsDeleted { get; set; }

    public string? CacheMethod { get; set; }

    public string? ModuleTitle { get; set; }

    public string? Header { get; set; }

    public string? Footer { get; set; }

    public string? CultureCode { get; set; }

    public Guid UniqueId { get; set; }

    public Guid VersionGuid { get; set; }

    public Guid? DefaultLanguageGuid { get; set; }

    public Guid LocalizedVersionGuid { get; set; }

    public ICollection<TabModuleSetting> ModuleSettings { get; set; } = new List<TabModuleSetting>();
}

public class TabModuleTypeConfiguration : IEntityTypeConfiguration<TabModule>
{
    public void Configure(EntityTypeBuilder<TabModule> builder)
    {
        builder.ToTable("TabModules");
        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.Id)
            .HasColumnName("TabModuleID");

        builder.Property(tm => tm.ModuleId)
            .HasColumnName("ModuleID");

        builder.Property(tm => tm.WebSliceTtl)
            .HasColumnName("WebSliceTTL");

        builder.Property(tms => tms.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(tms => tms.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(tm => tm.Tab)
            .WithMany(t => t.TabModules)
            .HasForeignKey(tm => tm.TabId)
            .HasPrincipalKey(t => t.Id)
            .IsRequired();

        builder.HasOne(tm => tm.Module)
            .WithMany(m => m.TabModules)
            .HasForeignKey(tm => tm.ModuleId)
            .HasPrincipalKey(m => m.Id)
            .IsRequired();
    }
}
