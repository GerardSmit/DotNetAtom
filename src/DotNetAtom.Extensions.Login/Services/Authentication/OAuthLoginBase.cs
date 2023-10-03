using System;
using System.Collections.Specialized;

namespace DotNetAtom.Services.Authentication;

public class OAuthLoginBase: AuthenticationLoginBase
{
	/// <inheritdoc/>
	public override bool Enabled => true;

	protected virtual void AddCustomProperties(NameValueCollection properties)
	{
	}
}
