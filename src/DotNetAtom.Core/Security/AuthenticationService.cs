using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Security;

public class AuthenticationService(IAuthenticationRepository repository) : IAuthenticationService
{
	public IReadOnlyCollection<IAuthenticationInfo> Authentications { get; private set; } = Array.Empty<IAuthenticationInfo>();

	public IReadOnlyCollection<IAuthenticationInfo> EnabledAuthentications { get; private set; } = Array.Empty<IAuthenticationInfo>();

	public IAuthenticationInfo? GetAuthenticationSystem(string dnn)
	{
		return Authentications.FirstOrDefault(x => x.AuthenticationType == dnn);
	}

	public async ValueTask LoadAsync()
	{
		Authentications = await repository.GetAuthenticationServices();
		EnabledAuthentications = Authentications.Where(x => x.IsEnabled).ToArray();
	}
}
