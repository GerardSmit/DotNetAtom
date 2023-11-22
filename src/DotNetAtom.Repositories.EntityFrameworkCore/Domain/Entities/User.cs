using System;
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

    public int? AffiliateId { get; set; }

    public bool IsSuperUser { get; set; }

    public string DisplayName { get; set; } = default!;

    public AspNetUser AspNetUser { get; set; } = null!;

    public Guid? PasswordResetToken { get; set; }

    public DateTime? PasswordResetExpiration { get; set; }


    public string? LastIpAddress { get; set; }

    public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

    public ICollection<TabPermission> TabPermissions { get; set; } = new List<TabPermission>();

    public ICollection<ModulePermission> ModulePermissions { get; set; } = new List<ModulePermission>();

    public ICollection<UserPortal> UserPortals { get; set; } = new List<UserPortal>();
}

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(pa => pa.Id);

        builder.Property(p => p.Id)
            .HasColumnName("UserID");

        builder.Property(u => u.LastIpAddress)
            .HasColumnName("LastIPAddress");

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

        builder.HasMany(u => u.UserPortals)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();
    }
}
