using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Permission : IEntity, ITimestamp
{
    public int Id { get; set; }
        
    public string PermissionCode { get; set; }
        
    public int ModuleDefinitionId { get; set; }
        
    public string PermissionKey { get; set; }
        
    public string PermissionName { get; set; }
        
    public int ViewOrder { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }
        
    public ICollection<ModulePermission> ModulePermissions { get; set; } = new List<ModulePermission>();
        
    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();
}

public class PermissionTypeConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permission");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("PermissionID");

        builder.Property(m => m.ModuleDefinitionId)
            .HasColumnName("ModuleDefID");

        builder.Property(t => t.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(t => t.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");
    }
}
