namespace DotNetAtom.Localization;

public interface ILocalizationService
{
	string GetString(string resourceName, string? localResourceFile = null);
}
