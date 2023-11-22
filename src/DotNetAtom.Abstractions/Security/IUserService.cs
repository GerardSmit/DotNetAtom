using System;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Security.Membership;

namespace DotNetAtom.Security;

public interface IUserService
{
	/// <summary>
	/// Authenticates a user with the given username and password.
	/// </summary>
	/// <param name="applicationId">The application ID, which can be retrieved from <see cref="IAtomContext.ApplicationId"/>.</param>
	/// <param name="portalId">The portal ID of the portal to authenticate the user against.</param>
	/// <param name="username">Username of the user to authenticate.</param>
	/// <param name="password">Password of the user to authenticate.</param>
	/// <returns>A <see cref="LoginResult"/> object containing the result of the authentication.</returns>
	Task<LoginResult> AuthenticateAsync(Guid applicationId, int portalId, string username, string password);

	/// <summary>
	/// Gets a user by their portal and username.
	/// </summary>
	/// <param name="portalId">The portal ID of the portal to get the user from.</param>
	/// <param name="username">Username of the user to get.</param>
	/// <returns>A <see cref="IUserInfo"/> object containing the user information or <see langword="null"/> if the user does not exist.</returns>
	Task<IUserInfo?> GetUserAsync(int portalId, string username);

	/// <summary>
	/// Gets a user by their portal and ID.
	/// </summary>
	/// <param name="portalId">The portal ID of the portal to get the user from.</param>
	/// <param name="userId">ID of the user to get.</param>
	/// <returns>A <see cref="IUserInfo"/> object containing the user information or <see langword="null"/> if the user does not exist.</returns>
	Task<IUserInfo?> GetUserAsync(int portalId, int userId);
}
