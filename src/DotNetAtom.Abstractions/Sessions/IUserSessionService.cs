using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Sessions;

/// <summary>
/// Represents a service that manages the current user.
/// The default implementation stores the current user in a cookie.
/// </summary>
public interface IUserSessionService
{
	/// <summary>Get the current user.</summary>
	/// <remarks>
	///	When the user is not logged in, this method returns an unauthenticated user with the ID of -1.
	/// </remarks>
	/// <returns>The current user.</returns>
	Task<IUserInfo> GetCurrentUserAsync();

	/// <summary>Sets the current user to the specified user.</summary>
	/// <param name="user">The user to set as the current user.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task SetCurrentUserAsync(IUserInfo user);
}
