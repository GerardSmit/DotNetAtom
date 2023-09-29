using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class ModuleDefinition : IEntity, ITimestamp
{
    /// <inheritdoc />
    public int Id { get; set; }

    public required string FriendlyName { get; set; }

    public int DesktopModuleId { get; set; }

    public DesktopModule DesktopModule { get; set; } = default!;

    public int DefaultCacheTime { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public ICollection<Module> Modules { get; set; } = new List<Module>();

    public ICollection<ModuleControl> ModuleControls { get; set; } = new List<ModuleControl>();
}

public class ModuleDefinitionEntityConfiguration : IEntityTypeConfiguration<ModuleDefinition>
{
    public void Configure(EntityTypeBuilder<ModuleDefinition> builder)
    {
        builder.ToTable("ModuleDefinitions");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("ModuleDefID");

        builder.Property(m => m.DesktopModuleId)
            .HasColumnName("DesktopModuleID");

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(m => m.DesktopModule)
            .WithMany(p => p.ModuleDefinitions)
            .HasForeignKey(pa => pa.DesktopModuleId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();
    }
}
