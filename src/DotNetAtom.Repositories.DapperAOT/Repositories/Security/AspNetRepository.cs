using System;
using System.Threading.Tasks;
using Dapper;
using DotNetAtom.Entities;
using DotNetAtom.Security;

namespace DotNetAtom.Repositories.DapperAOT.Repositories.Security;

internal class AspNetRepository : IAspNetRepository
{
	private readonly ConnectionFactory _connectionFactory;

	public AspNetRepository(ConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async Task<IAspNetUser?> GetUserAsync(Guid applicationId, Guid userId)
	{
		await using var container = _connectionFactory.CreateConnection();

		return await container.SqlConnection.QueryFirstAsync<AspNetUser>(
				"""
				SELECT *
				FROM aspnet_Users
				WHERE ApplicationId = @ApplicationId AND UserId = @UserId
				""",
				new
				{
					ApplicationId = applicationId,
					UserId = userId
				});
	}

	public async Task<IAspNetUser?> GetUserAsync(Guid applicationId, string username)
	{
		await using var container = _connectionFactory.CreateConnection();

		return await container.SqlConnection.QueryFirstAsync<AspNetUser>(
				"""
				SELECT *
				FROM aspnet_Users
				WHERE ApplicationId = @ApplicationId AND UserName = @Username
				""",
				new
				{
					ApplicationId = applicationId,
					Username = username
				});
	}

	public async Task<IAspNetMembership?> GetMembershipAsync(Guid applicationId, Guid userId)
	{
		await using var container = _connectionFactory.CreateConnection();

		return await container.SqlConnection.QueryFirstAsync<AspNetMembership>(
				"""
				SELECT *
				FROM aspnet_Membership
				WHERE ApplicationId = @ApplicationId AND UserId = @UserId
				""",
				new
				{
					ApplicationId = applicationId,
					UserId = userId
				});
	}
}
