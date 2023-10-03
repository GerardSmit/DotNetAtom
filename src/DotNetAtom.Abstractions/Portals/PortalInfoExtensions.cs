using System.Diagnostics.CodeAnalysis;

namespace DotNetAtom.Portals;

public static class PortalInfoExtensions
{
	[return: NotNullIfNotNull("defaultValue")]
	public static string? GetPortalSetting(this IPortalInfo portalInfo, string key, string? defaultValue = null)
	{
		return portalInfo.Settings.TryGetValue(key, out var value) ? value : defaultValue;
	}
}
