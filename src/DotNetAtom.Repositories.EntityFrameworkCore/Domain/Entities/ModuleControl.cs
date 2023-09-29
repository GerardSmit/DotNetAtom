using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class ModuleControl : IEntity, ITimestamp
{
    public int Id { get; set; }

    public int? ModuleDefinitionId { get; set; }

    public ModuleDefinition ModuleDefinition { get; set; } = default!;

    public string? ControlKey { get; set; }

    public string? ControlTitle { get; set; }

    public string? ControlSrc { get; set; }

    public string? IconFile { get; set; }

    public SecurityAccessLevel ControlType { get; set; }

    public int? ViewOrder { get; set; }

    public string? HelpUrl { get; set; }

    public bool SupportsPartialRendering { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public bool SupportsPopUps { get; set; }
}

public class ModuleControlTypeConfiguration : IEntityTypeConfiguration<ModuleControl>
{
    public void Configure(EntityTypeBuilder<ModuleControl> builder)
    {
        builder.ToTable("ModuleControls");
        builder.HasKey(m => m.Id);

        builder.Property(t => t.Id)
            .HasColumnName("ModuleControlID")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.ModuleDefinitionId)
            .HasColumnName("ModuleDefID")
            .IsRequired();

        builder.Property(t => t.ControlKey)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.ControlTitle)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.ControlSrc)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.IconFile)
            .HasMaxLength(100);

        builder.Property(t => t.HelpUrl)
            .HasMaxLength(200);

        builder.HasOne(t => t.ModuleDefinition)
            .WithMany(t => t.ModuleControls)
            .HasForeignKey(t => t.ModuleDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
