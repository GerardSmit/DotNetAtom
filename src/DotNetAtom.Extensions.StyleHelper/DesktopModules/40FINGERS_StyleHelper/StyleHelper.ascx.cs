using System;
using System.Text.RegularExpressions;
using DotNetAtom.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebFormsCore.Features;

namespace DotNetAtom.DesktopModules._40FINGERS_StyleHelper;

public partial class StyleHelper : PortalModuleBase
{
	private readonly ILogger<StyleHelper> _logger;

	public StyleHelper(ILogger<StyleHelper> logger)
	{
		_logger = logger;
	}

	public string? RemoveCssFile { get; set; }

	protected override void OnInit(EventArgs args)
	{
		base.OnInit(args);

		if (RemoveCssFile is null or "")
		{
			return;
		}

		var collection = Context.RequestServices.GetService<IClientDependencyCollection>();

		if (collection is null)
		{
			return;
		}

		if (RemoveCssFile == "/")
		{
			collection.RemoveAll(x => x.FilePath is not null && x.FilePath.StartsWith("/"));
		}
		else if (Regex.Escape(RemoveCssFile) == RemoveCssFile)
		{
			collection.RemoveAll(x => x.FilePath is not null && x.FilePath.IndexOf(RemoveCssFile, StringComparison.OrdinalIgnoreCase) != -1);
		}
		else
		{
			try
			{
				var regex = new Regex(RemoveCssFile, RegexOptions.IgnoreCase);

				collection.RemoveAll(x => x.FilePath is not null && regex.IsMatch(x.FilePath));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while removing CSS files");
			}
		}
	}
}
