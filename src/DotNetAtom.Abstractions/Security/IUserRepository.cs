using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Security;

public interface IUserRepository
{
	Task<IUserInfo?> GetUserAsync(int portalId, string username);

	Task<IUserInfo?> GetUserAsync(int portalId, int userId);
}
