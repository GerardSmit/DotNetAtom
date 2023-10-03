using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;

namespace DotNetAtom.Security;

public interface IAuthenticationService
{
	IReadOnlyCollection<IAuthenticationInfo> Authentications { get; }

	IReadOnlyCollection<IAuthenticationInfo> EnabledAuthentications { get; }

	IAuthenticationInfo? GetAuthenticationSystem(string dnn);

	ValueTask LoadAsync();
}
