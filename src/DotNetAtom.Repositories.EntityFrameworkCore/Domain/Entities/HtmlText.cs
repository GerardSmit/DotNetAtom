using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class HtmlText : IEntity, ITimestamp
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public Module Module { get; set; }

    public string? Content { get; set; }

    public int? Version { get; set; }

    public int? StateId { get; set; }

    public bool? IsPublished { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public string? Summary { get; set; }
}

public class HtmlTextTypeConfiguration : IEntityTypeConfiguration<HtmlText>
{
    public void Configure(EntityTypeBuilder<HtmlText> builder)
    {
        builder.ToTable("HtmlText");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("ItemID");

        builder.Property(m => m.ModuleId)
            .HasColumnName("ModuleId");

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(ht => ht.Module)
            .WithMany(m => m.HtmlTexts)
            .HasForeignKey(m => m.ModuleId)
            .HasPrincipalKey(md => md.Id)
            .IsRequired();
    }
}
