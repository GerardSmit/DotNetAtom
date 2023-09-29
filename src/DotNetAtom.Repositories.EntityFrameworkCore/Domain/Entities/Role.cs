using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Role
{
    public int Id { get; set; }

    public int PortalId { get; set; }

    public required string Name { get; set; }

    public Portal Portal { get; set; } = default!;

    public bool IsPublic { get; set; }

    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();

    public ICollection<ModulePermission> ModulePermissions { get; set; } = new List<ModulePermission>();
}

public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(pa => pa.Id);

        builder.Property(p => p.Id)
            .HasColumnName("RoleID");

        builder.Property(p => p.PortalId)
            .HasColumnType("PortalID");

        builder.Property(p => p.Name)
            .HasColumnName("RoleName");

        builder.HasOne(pa => pa.Portal)
            .WithMany(p => p.Roles)
            .HasForeignKey(pa => pa.PortalId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired();

        builder.HasMany(u => u.TabPermissions)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();

        builder.HasMany(u => u.ModulePermissions)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();
    }
}
