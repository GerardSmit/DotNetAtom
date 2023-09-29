using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class User : IEntity
{
    public int Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string? Email { get; set; }

    public bool IsSuperUser { get; set; }

    public AspNetUser AspNetUser { get; set; } = null!;

    public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();

    public ICollection<ModulePermission> ModulePermissions { get; set; } = new List<ModulePermission>();
}

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(pa => pa.Id);

        builder.Property(p => p.Id)
            .HasColumnName("UserID");

        builder.HasOne(u => u.AspNetUser)
            .WithMany(u => u.User)
            .HasForeignKey(u => u.Username)
            .HasPrincipalKey(u => u.Username)
            .IsRequired();

        builder.HasMany(u => u.Roles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();

        builder.HasMany(u => u.TabPermissions)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();

        builder.HasMany(u => u.ModulePermissions)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();
    }
}
