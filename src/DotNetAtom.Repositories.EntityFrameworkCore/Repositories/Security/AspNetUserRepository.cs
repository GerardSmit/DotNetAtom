using System;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Security;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.EntityFrameworkCore.Repositories.AspNetUser;

public class AspNetUserRepository : IAspNetUserRepository
{
	private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

	public AspNetUserRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async Task<IAspNetUser?> GetUserAsync(Guid applicationId, Guid userId)
	{
		await using var context = await _dbContextFactory.CreateDbContextAsync();

		return await context.AspNetUser
			.SingleOrDefaultAsync(u => u.ApplicationId == applicationId && u.UserId == userId);
	}

	public async Task<IAspNetUser?> GetUserAsync(Guid applicationId, string username)
	{
		await using var context = await _dbContextFactory.CreateDbContextAsync();

		return await context.AspNetUser
			.SingleOrDefaultAsync(u => u.ApplicationId == applicationId && u.Username == username);
	}

	public async Task<IAspNetMembership?> GetMembershipAsync(Guid applicationId, Guid userId)
	{
		await using var context = await _dbContextFactory.CreateDbContextAsync();

		return await context.AspNetMembership
			.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.UserId == userId);
	}
}
