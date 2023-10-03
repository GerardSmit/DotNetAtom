using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Security;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.Repositories.EntityFrameworkCore.Repositories.Security;

public class AuthenticationRepository : IAuthenticationRepository
{
	private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

	public AuthenticationRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async ValueTask<IReadOnlyList<IAuthenticationInfo>> GetAuthenticationServices()
	{
		await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

		return await dbContext.Authentication.ToListAsync();
	}
}
