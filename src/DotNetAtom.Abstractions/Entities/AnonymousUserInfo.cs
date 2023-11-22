using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNetAtom.Entities;

public class AnonymousUserInfo : IUserInfo
{
	public static readonly AnonymousUserInfo Instance = new();

	private static readonly string[] DefaultRoles = { "Unauthenticated Users" };

	public int? AffiliateId
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public string DisplayName
	{
		get => "ANONYMOUS";
		set => ThrowReadOnlyException();
	}

	public string? Email
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public string FirstName
	{
		get => "ANONYMOUS";
		set => ThrowReadOnlyException();
	}

	public bool HasAgreedToTerms
	{
		get => false;
		set => ThrowReadOnlyException();
	}

	public DateTime? HasAgreedToTermsOn
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public bool IsAdmin
	{
		get => false;
		set => ThrowReadOnlyException();
	}

	public bool IsDeleted
	{
		get => false;
		set => ThrowReadOnlyException();
	}

	public bool IsSuperUser
	{
		get => false;
		set => ThrowReadOnlyException();
	}

	public string? LastIPAddress
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public string LastName
	{
		get => "ANONYMOUS";
		set => ThrowReadOnlyException();
	}

	public DateTime? PasswordResetExpiration
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public Guid? PasswordResetToken
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public int PortalId
	{
		get => 0;
		set => ThrowReadOnlyException();
	}

	public bool RequestsRemoval
	{
		get => false;
		set => ThrowReadOnlyException();
	}

	public string[] Roles
	{
		get => DefaultRoles;
		set => ThrowReadOnlyException();
	}

	public int UserId
	{
		get => -1;
		set => ThrowReadOnlyException();
	}

	public string Username
	{
		get => string.Empty;
		set => ThrowReadOnlyException();
	}

	public string? VanityUrl
	{
		get => null;
		set => ThrowReadOnlyException();
	}

	public bool IsInRole(string role)
	{
		return role.Equals("Unauthenticated Users", StringComparison.OrdinalIgnoreCase);
	}

	public DateTime LocalTime()
	{
		return DateTime.Now;
	}

	public DateTime LocalTime(DateTime utcTime)
	{
		return utcTime.ToLocalTime();
	}

	[DoesNotReturn]
	private void ThrowReadOnlyException()
	{
		throw new InvalidOperationException("Cannot modify anonymous user");
	}
}
