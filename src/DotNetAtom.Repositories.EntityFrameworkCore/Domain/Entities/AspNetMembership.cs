using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class AspNetMembership
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public string Password { get; set; } = default!;

    public string PasswordSalt { get; set; } = default!;

    public string? Email { get; set; }

    public string? LoweredEmail { get; set; }

    public AspNetUser User { get; set; } = default!;
}

public class AspNetMembershipTypeConfiguration : IEntityTypeConfiguration<AspNetMembership>
{
    public void Configure(EntityTypeBuilder<AspNetMembership> builder)
    {
        builder.ToTable("aspnet_Membership");
        builder.HasKey(u => u.UserId);
    }
}
