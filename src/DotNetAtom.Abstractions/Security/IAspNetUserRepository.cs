using System;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Security;

public interface IAspNetUserRepository
{
	Task<IAspNetUser?> GetUserAsync(Guid applicationId, Guid userId);

	Task<IAspNetUser?> GetUserAsync(Guid applicationId, string username);

	Task<IAspNetMembership?> GetMembershipAsync(Guid applicationId, Guid userId);
}
