namespace DotNetAtom.Entities;

public interface IAuthenticationInfo
{
	bool IsEnabled { get; }

	string AuthenticationType { get; }

	string LoginControlSrc { get; }

	string? SettingsControlSrc { get; }

	string? LogoffControlSrc { get; }
}
