using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class TabPermission : ITimestamp
{
    public int Id { get; set; }

    public int TabId { get; set; }

    public Tab Tab { get; set; } = default!;

    public int PermissionId { get; set; }

    public Permission Permission { get; set; } = default!;

    public bool AllowAccess { get; set; }

    public int? RoleId { get; set; }

    public Role? Role { get; set; }

    public int? UserId { get; set; }

    public User? User { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
}

public class TabPermissionTypeConfiguration : IEntityTypeConfiguration<TabPermission>
{
    public void Configure(EntityTypeBuilder<TabPermission> builder)
    {
        builder.ToTable("TabPermission");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("TabPermissionID");

        builder.Property(m => m.TabId)
            .HasColumnName("TabID");

        builder.Property(t => t.PermissionId)
            .HasColumnName("PermissionID");

        builder.Property(t => t.RoleId)
            .HasColumnName("RoleID")
            .IsRequired(false);

        builder.Property(t => t.UserId)
            .HasColumnName("UserID")
            .IsRequired(false);

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.HasOne(ht => ht.Tab)
            .WithMany(m => m.TabPermissions)
            .HasForeignKey(m => m.TabId)
            .HasPrincipalKey(md => md.Id)
            .IsRequired();

        builder.HasOne(ht => ht.Permission)
            .WithMany(m => m.TabPermissions)
            .HasForeignKey(m => m.PermissionId)
            .HasPrincipalKey(md => md.Id)
            .IsRequired();
    }
}
