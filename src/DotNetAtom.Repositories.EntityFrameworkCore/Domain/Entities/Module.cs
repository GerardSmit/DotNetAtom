using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Module : IEntity, ITimestamp
{
    /// <inheritdoc />
    public int Id { get; set; }

    public int ModuleDefId { get; set; }

    public ModuleDefinition ModuleDef { get; set; }

    public bool AllTabs { get; set; }

    public bool IsDeleted { get; set; }

    public bool? InheritViewPermissions { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? PortalId { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public DateTime? LastContentModifiedOnDate { get; set; }

    public int? ContentItemId { get; set; }

    public ICollection<TabModule> TabModules { get; set; } = new List<TabModule>();

    public ICollection<HtmlText> HtmlTexts { get; set; } = new List<HtmlText>();

    public ICollection<ModulePermission> Permissions { get; set; } = new List<ModulePermission>();
}

public class ModuleTypeConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Modules");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("ModuleId");

        builder.Property(m => m.ModuleDefId)
            .HasColumnName("ModuleDefID");

        builder.Property(t => t.PortalId)
            .HasColumnName("PortalID");

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.Property(t => t.ContentItemId)
            .HasColumnName("ContentItemID");

        builder.HasOne(m => m.ModuleDef)
            .WithMany(md => md.Modules)
            .HasForeignKey(m => m.ModuleDefId)
            .HasPrincipalKey(md => md.Id)
            .IsRequired();
    }
}
