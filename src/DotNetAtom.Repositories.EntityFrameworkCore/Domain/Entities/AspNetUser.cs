using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class AspNetUser : IAspNetUser
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public string Username { get; set; }

    public string LoweredUsername { get; set; }

    public string? MobileAlias { get; set; }

    public bool IsAnonymous { get; set; }

    public DateTime LastActivityDate { get; set; }

    public IList<User> User { get; set; }

    public ICollection<AspNetMembership> Memberships { get; set; } = new List<AspNetMembership>();
}

public class AspNetUserTypeConfiguration : IEntityTypeConfiguration<AspNetUser>
{
    public void Configure(EntityTypeBuilder<AspNetUser> builder)
    {
        builder.ToTable("aspnet_Users");
        builder.HasKey(p => p.UserId);

        builder.Property(p => p.Username)
            .HasColumnName("UserName");

        builder.Property(p => p.LoweredUsername)
            .HasColumnName("LoweredUserName");

        builder.HasMany(u => u.Memberships)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .HasPrincipalKey(u => u.UserId)
            .IsRequired();
    }
}
