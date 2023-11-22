using System;

namespace DotNetAtom.Entities;

public interface IAspNetMembership
{
	Guid ApplicationId { get; set; }

	Guid UserId { get; set; }

	string Password { get; set; }

	int PasswordFormat { get; set; }

	string PasswordSalt { get; set; }

	string? MobilePin { get; set; }

	string? Email { get; set; }

	string? LoweredEmail { get; set; }

	string? PasswordQuestion { get; set; }

	string? PasswordAnswer { get; set; }

	bool IsApproved { get; set; }

	bool IsLockedOut { get; set; }

	DateTime CreateDate { get; set; }

	DateTime LastLoginDate { get; set; }

	DateTime LastPasswordChangedDate { get; set; }

	DateTime LastLockoutDate { get; set; }

	int FailedPasswordAttemptCount { get; set; }

	DateTime FailedPasswordAttemptWindowStart { get; set; }

	int FailedPasswordAnswerAttemptCount { get; set; }

	DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

	string? Comment { get; set; }
}
