using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DotNetAtom.Entities;
using DotNetAtom.Repositories.DapperAOT.Repositories.Security;
using DotNetAtom.Security;

namespace DotNetAtom.Repositories.DapperAOT.Repositories.Users;

internal class UserRepository : IUserRepository
{
	private readonly ConnectionFactory _connectionFactory;

	public UserRepository(ConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async Task<IUserInfo?> GetUserAsync(int portalId, string username)
	{
		await using var container = _connectionFactory.CreateConnection();
		var connection = container.SqlConnection;

		var user = await connection.QuerySingleOrDefaultAsync<UserInfo>(
			"""
			SELECT
				u.Email,
				u.Username,
				u.AffiliateId,
				u.DisplayName,
				u.FirstName,
				u.UserId,
				up.PortalId,
				u.IsSuperUser,
				IIF(u.IsSuperUser = 1 OR ur.UserRoleID IS NOT NULL, 1, 0) AS IsAdmin,
				up.IsDeleted,
				u.LastName,
				u.LastIpAddress
			FROM UserPortals up
			INNER JOIN Users u ON u.UserID = up.UserId
			INNER JOIN Portals p ON p.PortalID = up.PortalId
			LEFT JOIN UserRoles ur ON ur.UserID = u.UserID AND ur.RoleID = p.AdministratorRoleId
			WHERE up.PortalId = @PortalId
			AND u.Username = @Username
			""",
			new
			{
				PortalId = portalId,
				Username = username
			});

		if (user == null)
		{
			return null;
		}

		var roles = await connection.QueryAsync<string>(
			"""
			SELECT r.RoleName
			FROM UserRoles ur
			INNER JOIN Roles r ON r.RoleID = ur.RoleID
			WHERE r.PortalID = @PortalId
			AND ur.UserID = @UserId
			""",
			new
			{
				PortalId = portalId,
				UserId = user.UserId
			});

		user.Roles = roles.ToArray();

		return user;
	}

	public async Task<IUserInfo?> GetUserAsync(int portalId, int userId)
	{
		await using var container = _connectionFactory.CreateConnection();
		var connection = container.SqlConnection;

		var user = await connection.QuerySingleOrDefaultAsync<UserInfo>(
			"""
			SELECT
				u.Email,
				u.Username,
				u.AffiliateId,
				u.DisplayName,
				u.FirstName,
				u.UserId,
				up.PortalId,
				u.IsSuperUser,
				IIF(u.IsSuperUser = 1 OR ur.UserRoleID IS NOT NULL, 1, 0) AS IsAdmin,
				up.IsDeleted,
				u.LastName,
				u.LastIpAddress
			FROM UserPortals up
			INNER JOIN Users u ON u.UserID = up.UserId
			INNER JOIN Portals p ON p.PortalID = up.PortalId
			LEFT JOIN UserRoles ur ON ur.UserID = u.UserID AND ur.RoleID = p.AdministratorRoleId
			WHERE up.PortalId = @PortalId
			AND u.UserId = @UserId
			""",
			new
			{
				PortalId = portalId,
				UserId = userId
			});

		if (user == null)
		{
			return null;
		}

		var roles = await connection.QueryAsync<string>(
			"""
			SELECT r.RoleName
			FROM UserRoles ur
			INNER JOIN Roles r ON r.RoleID = ur.RoleID
			WHERE r.PortalID = @PortalId
			AND ur.UserID = @UserId
			""",
			new
			{
				PortalId = portalId,
				UserId = user.UserId
			});

		user.Roles = roles.ToArray();

		return user;
	}
}
