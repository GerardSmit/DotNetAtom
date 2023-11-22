using System;

namespace DotNetAtom.Entities;

public interface IAspNetUser
{
	Guid ApplicationId { get; set; }

	Guid UserId { get; set; }

	string Username { get; set; }

	string LoweredUsername { get; set; }

	string? MobileAlias { get; set; }

	bool IsAnonymous { get; set; }

	DateTime LastActivityDate { get; set; }
}
