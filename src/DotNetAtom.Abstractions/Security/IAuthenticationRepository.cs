using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Security;

public interface IAuthenticationRepository
{
	ValueTask<IReadOnlyList<IAuthenticationInfo>> GetAuthenticationServices();
}
