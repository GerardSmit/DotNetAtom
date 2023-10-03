using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Authentication : IAuthenticationInfo, IEntity, ITimestamp
{
	public int Id { get; set; }

	public int PackageId { get; set; }

	public bool IsEnabled { get; set; }

	public string AuthenticationType { get; set; }

	public string LoginControlSrc { get; set; }

	public string? SettingsControlSrc { get; set; }

	public string? LogoffControlSrc { get; set; }

	public int? CreatedByUserId { get; set; }

	public DateTime? CreatedOnDate { get; set; }

	public int? LastModifiedByUserId { get; set; }

	public DateTime? LastModifiedOnDate { get; set; }
}

public class AuthenticationTypeConfiguration : IEntityTypeConfiguration<Authentication>
{
	public void Configure(EntityTypeBuilder<Authentication> builder)
	{
		builder.ToTable("Authentication");
		builder.HasKey(m => m.Id);

		builder.Property(t => t.Id)
			.HasColumnName("AuthenticationID");

		builder.Property(t => t.PackageId)
			.HasColumnName("PackageID");

		builder.Property(t => t.CreatedByUserId)
			.HasColumnName("CreatedByUserID");

		builder.Property(t => t.LastModifiedByUserId)
			.HasColumnName("LastModifiedByUserID");
	}
}
