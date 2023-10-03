using DotNetAtom.Entities;
using DotNetNuke.Services.Authentication;

namespace DotNetAtom.Security;

internal class AuthenticationInfoWrapper : IAuthenticationInfo
{
	private readonly AuthenticationInfo _authenticationInfo;

	public AuthenticationInfoWrapper(AuthenticationInfo authenticationInfo)
	{
		_authenticationInfo = authenticationInfo;
	}

	public bool IsEnabled => _authenticationInfo.IsEnabled;

	public string AuthenticationType => _authenticationInfo.AuthenticationType;

	public string LoginControlSrc => _authenticationInfo.LoginControlSrc;

	public string? SettingsControlSrc => _authenticationInfo.SettingsControlSrc;

	public string? LogoffControlSrc => _authenticationInfo.LogoffControlSrc;
}
