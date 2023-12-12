using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class UserPortal
{
	public int PortalId { get; set; }

	public Portal Portal { get; set; } = default!;

	public int UserId { get; set; }

	public User User { get; set; } = default!;

	public int UserPortalId { get; set; }

	public DateTime CreatedDate { get; set; }

	public bool Authorised { get; set; }

	public bool IsDeleted { get; set; }

	public bool RefreshRoles { get; set; }
}

public class UserPortalTypeConfiguration : IEntityTypeConfiguration<UserPortal>
{
	public void Configure(EntityTypeBuilder<UserPortal> builder)
	{
		builder.ToTable("UserPortals");
		builder.HasKey(pa => new { pa.UserId, pa.PortalId });
	}
}
