using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DotNetAtom.Localization;

public class LocalizationService : ILocalizationService
{
	private ConcurrentDictionary<string, Dictionary<string, string>> _localizationCache = new();

#if NET
	private static ReadOnlySpan<char> TextResourceNameSuffix => ".Text";
#endif

	public string GetString(string resourceName, string? localResourceFile = null)
	{
		if (localResourceFile is null)
		{
			return string.Empty;
		}

		var cache = _localizationCache.GetOrAdd(localResourceFile, LoadLocalizationFile);

		if (!resourceName.Contains('.'))
		{
#if NET
			resourceName = string.Create(resourceName.Length + TextResourceNameSuffix.Length, resourceName, static (span, state) =>
			{
				state.AsSpan().CopyTo(span);
				TextResourceNameSuffix.CopyTo(span.Slice(state.Length));
			});
#else
			resourceName = $"{resourceName}.Text";
#endif
		}

		return cache.TryGetValue(resourceName, out var value) ? value : string.Empty;
	}

	private Dictionary<string, string> LoadLocalizationFile(string arg)
	{
		var document = XDocument.Load(arg);
		var resources = new Dictionary<string, string>();
		var dataElements = document.Root?.Elements("data") ?? Enumerable.Empty<XElement>();

		foreach (var element in dataElements)
		{
			var name = element.Attribute("name")?.Value;
			var value = element.Element("value")?.Value;

			if (name is not null && value is not null)
			{
				resources.Add(name, value);
			}
		}

		return resources;
	}
}
