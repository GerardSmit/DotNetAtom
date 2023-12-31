﻿using System;

namespace DotNetAtom.Entities;

public interface IUserInfo
{
	/// <summary>Gets or sets the AffiliateId for this user.</summary>
	int? AffiliateId { get; set; }

	/// <summary>Gets or sets the Display Name.</summary>
	string DisplayName { get; set; }

	/// <summary>Gets or sets the Email Address.</summary>
	string? Email { get; set; }

	/// <summary>Gets or sets the First Name.</summary>
	string FirstName { get; set; }

	/// <summary>Gets a value indicating whether the user is in the portal's administrators role.</summary>
	bool IsAdmin { get; }

	/// <summary>Gets or sets a value indicating whether the User is deleted.</summary>
	bool IsDeleted { get; set; }

	/// <summary>Gets or sets a value indicating whether the User is a SuperUser.</summary>
	bool IsSuperUser { get; set; }

	/// <summary>Gets or sets the Last IP address used by user.</summary>
	string? LastIPAddress { get; set; }

	/// <summary>Gets or sets the Last Name.</summary>
	string LastName { get; set; }

	/// <summary>Gets or sets the PortalId.</summary>
	int PortalId { get; set; }

	/// <summary>Gets or sets the roles.</summary>
	string[] Roles { get; set; }

	/// <summary>Gets or sets the User Id.</summary>
	int UserId { get; set; }

	/// <summary>Gets or sets the User Name.</summary>
	string Username { get; set; }

	/// <summary>IsInRole determines whether the user is in the role passed.</summary>
	/// <param name="role">The role to check.</param>
	/// <returns>A Boolean indicating success or failure.</returns>
	bool IsInRole(string role);

	/// <summary>Gets current time in User's timezone.</summary>
	/// <returns>The local time for the user.</returns>
	DateTime LocalTime();

	/// <summary>Convert utc time in User's timezone.</summary>
	/// <param name="utcTime">Utc time to convert.</param>
	/// <returns>The local time for the user.</returns>
	DateTime LocalTime(DateTime utcTime);
}
