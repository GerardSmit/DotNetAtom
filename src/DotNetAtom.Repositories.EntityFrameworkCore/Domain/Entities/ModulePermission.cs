using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class ModulePermission : IEntity, ITimestamp
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public Module Module { get; set; }

    public int PermissionId { get; set; }

    public Permission Permission { get; set; }

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

public class ModulePermissionTypeConfiguration : IEntityTypeConfiguration<ModulePermission>
{
    public void Configure(EntityTypeBuilder<ModulePermission> builder)
    {
        builder.ToTable("ModulePermission");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("ModulePermissionID");

        builder.Property(m => m.ModuleId)
            .HasColumnName("ModuleID");

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

        builder.HasOne(ht => ht.Module)
            .WithMany(m => m.Permissions)
            .HasForeignKey(m => m.ModuleId)
            .HasPrincipalKey(md => md.Id)
            .IsRequired();

        builder.HasOne(ht => ht.Permission)
            .WithMany(m => m.ModulePermissions)
            .HasForeignKey(m => m.PermissionId)
            .HasPrincipalKey(md => md.Id)
            .IsRequired();
    }
}
