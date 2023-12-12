using System;
using System.Linq;
using DotNetAtom.Entities;

namespace DotNetAtom.Repositories.DapperAOT.Repositories.Security;

public class UserInfo : IUserInfo
{
	public int? AffiliateId { get; set; }

	public string DisplayName { get; set; }

	public string? Email { get; set; }

	public string FirstName { get; set; }

	public bool HasAgreedToTerms { get; set; }

	public DateTime? HasAgreedToTermsOn { get; set; }

	public bool IsAdmin { get; set; }

	public bool IsDeleted { get; set; }

	public bool IsSuperUser { get; set; }

	public string? LastIPAddress { get; set; }

	public string LastName { get; set; }

	public DateTime? PasswordResetExpiration { get; set; }

	public Guid? PasswordResetToken { get; set; }

	public int PortalId { get; set; }

	public bool RequestsRemoval { get; set; }

	public string[] Roles { get; set; }

	public int UserId { get; set; }

	public string Username { get; set; }

	public string? VanityUrl { get; set; }

	public bool IsInRole(string role)
	{
		return Roles.Contains(role);
	}

	public DateTime LocalTime()
	{
		// TODO: Implement time zone conversion
		return DateTime.Now;
	}

	public DateTime LocalTime(DateTime utcTime)
	{
		// TODO: Implement time zone conversion
		return utcTime.ToLocalTime();
	}
}
