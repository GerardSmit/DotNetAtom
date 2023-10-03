using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetNuke.Services.Authentication;

namespace DotNetAtom.Security;

public class AuthenticationRepository : IAuthenticationRepository
{
	public ValueTask<IReadOnlyList<IAuthenticationInfo>> GetAuthenticationServices()
	{
		return new ValueTask<IReadOnlyList<IAuthenticationInfo>>(
			AuthenticationController.GetEnabledAuthenticationServices()
				.Select(authentication => new AuthenticationInfoWrapper(authentication))
				.ToArray()
		);
	}
}
