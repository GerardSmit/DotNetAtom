using System;
using Dapper;

namespace DotNetAtom.Entities;

public class Authentication : IAuthenticationInfo
{
	[DbValue(Name = "AuthenticationID")]
	public int Id { get; set; }

	[DbValue(Name = "PackageID")]
	public int PackageId { get; set; }

	public bool IsEnabled { get; set; }

	public string AuthenticationType { get; set; }

	public string LoginControlSrc { get; set; }

	public string? SettingsControlSrc { get; set; }

	public string? LogoffControlSrc { get; set; }

	[DbValue(Name = "CreatedByUserID")]
	public int? CreatedByUserId { get; set; }

	public DateTime? CreatedOnDate { get; set; }

	[DbValue(Name = "LastModifiedByUserID")]
	public int? LastModifiedByUserId { get; set; }

	public DateTime? LastModifiedOnDate { get; set; }
}
