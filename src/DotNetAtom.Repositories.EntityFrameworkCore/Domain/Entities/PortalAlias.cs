using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class PortalAlias : IEntity, ITimestamp
{
    public int Id { get; set; }

    public int PortalId { get; set; }

    public Portal Portal { get; set; }

    public string? HttpAlias { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}

public class PortalAliasTypeConfiguration : IEntityTypeConfiguration<PortalAlias>
{
    public void Configure(EntityTypeBuilder<PortalAlias> builder)
    {
        builder.ToTable("PortalAlias");
        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.Id)
            .HasColumnName("PortalAliasID");

        builder.Property(pa => pa.PortalId)
            .HasColumnName("PortalID");

        builder.Property(pa => pa.HttpAlias)
            .HasColumnName("HTTPAlias");

        builder.Property(pa => pa.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(pa => pa.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(pa => pa.Portal)
            .WithMany(p => p.Aliases)
            .HasForeignKey(pa => pa.PortalId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();
    }
}
