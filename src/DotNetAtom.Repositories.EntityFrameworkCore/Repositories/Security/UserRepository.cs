using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Security;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.Repositories.EntityFrameworkCore.Repositories.Security;

public class UserRepository : IUserRepository
{
	private readonly IDbContextFactory<AtomDbContext> _dbContextFactory;

	public UserRepository(IDbContextFactory<AtomDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}

	public async Task<IUserInfo?> GetUserAsync(int portalId, string username)
	{
		await using var context = await _dbContextFactory.CreateDbContextAsync();

		var query = context.UserPortal.Where(u => u.PortalId == portalId && u.User.Username == username);

		return await ToUserInfo(query).FirstOrDefaultAsync();
	}

	public async Task<IUserInfo?> GetUserAsync(int portalId, int userId)
	{
		await using var context = await _dbContextFactory.CreateDbContextAsync();

		var query = context.UserPortal.Where(u => u.PortalId == portalId && u.UserId == userId);

		return await ToUserInfo(query).FirstOrDefaultAsync();
	}

	private IQueryable<UserInfo> ToUserInfo(IQueryable<UserPortal> userPortal)
	{
		return userPortal
			.AsSplitQuery()
			.Select(u => new UserInfo
			{
				Email = u.User.Email,
				Roles = u.User.Roles.Select(i => i.Role.Name).ToArray(),
				Username = u.User.Username,
				AffiliateId = u.User.AffiliateId,
				DisplayName = u.User.DisplayName,
				FirstName = u.User.FirstName,
				HasAgreedToTerms = u.HasAgreedToTerms,
				UserId = u.User.Id,
				PortalId = u.PortalId,
				IsSuperUser = u.User.IsSuperUser,
				IsAdmin = u.User.IsSuperUser || u.User.Roles.Any(r => r.RoleId == u.Portal.AdministratorRoleId),
				IsDeleted = u.IsDeleted,
				LastName = u.User.LastName,
				RequestsRemoval = u.RequestsRemoval,
				VanityUrl = u.VanityUrl,
				PasswordResetExpiration = u.User.PasswordResetExpiration,
				PasswordResetToken = u.User.PasswordResetToken,
				LastIPAddress = u.User.LastIpAddress,
				HasAgreedToTermsOn = u.HasAgreedToTermsOn
			});
	}
}
