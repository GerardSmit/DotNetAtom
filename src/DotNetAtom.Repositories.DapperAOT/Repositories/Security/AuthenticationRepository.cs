using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DotNetAtom.Entities;
using DotNetAtom.Security;

namespace DotNetAtom.Repositories.DapperAOT.Repositories.Security;

internal class AuthenticationRepository : IAuthenticationRepository
{
	private readonly ConnectionFactory _connectionFactory;

	public AuthenticationRepository(ConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async ValueTask<IReadOnlyList<IAuthenticationInfo>> GetAuthenticationServices()
	{
		await using var container = _connectionFactory.CreateConnection();
		var connection = container.SqlConnection;

		return await connection.QueryUnbufferedAsync<Authentication>(
				"""
				SELECT *
				FROM Authentication
				""")
			.ToListAsync();
	}
}
