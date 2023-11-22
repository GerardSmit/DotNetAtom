using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Providers;
using DotNetAtom.Security.Membership;

namespace DotNetAtom.Security;

public class UserService(
	IAspNetUserRepository aspNetUserRepository,
	IEnumerable<IPasswordHasher> passwordHashers,
	IUserRepository userRepository
) : IUserService
{
	public async Task<LoginResult> AuthenticateAsync(Guid applicationId, int portalId, string username, string password)
	{
		var user = await aspNetUserRepository.GetUserAsync(applicationId, username);

		if (user is null)
		{
			return new LoginResult(UserLoginStatus.LOGIN_FAILURE, null);
		}

		var membership = await aspNetUserRepository.GetMembershipAsync(applicationId, user.UserId);

		if (membership is null)
		{
			return new LoginResult(UserLoginStatus.LOGIN_FAILURE, null);
		}

		var passwordHasher = passwordHashers.FirstOrDefault(x => x.Format == membership.PasswordFormat);

		if (passwordHasher is null)
		{
			throw new InvalidOperationException($"No password hasher found for format '{membership.PasswordFormat}'.");
		}

		var passwordValid = passwordHasher.Validate(
			membership.PasswordFormat,
			membership.Password,
			password, membership.PasswordSalt);

		if (!passwordValid)
		{
			return new LoginResult(UserLoginStatus.LOGIN_FAILURE, null);
		}

		var userInfo = await GetUserAsync(portalId, username);

		if (userInfo is null)
		{
			// The user does not exist in the portal.
			return new LoginResult(UserLoginStatus.LOGIN_FAILURE, null);
		}

		return new LoginResult(UserLoginStatus.LOGIN_SUCCESS, userInfo);
	}

	public Task<IUserInfo?> GetUserAsync(int portalId, string username)
	{
		return userRepository.GetUserAsync(portalId, username);
	}

	public Task<IUserInfo?> GetUserAsync(int portalId, int userId)
	{
		return userRepository.GetUserAsync(portalId, userId);
	}
}
