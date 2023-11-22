using System;
using System.Collections.Generic;
using System.Linq;
using DotNetAtom.Collections;
using DotNetAtom.Entities;
using DotNetAtom.UI.Skins;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;

namespace DotNetAtom.Tabs;

public class TabInfoWrapper : ITabInfo
{
	private readonly TabInfo _tab;
	private HashtableDictionary<string, string>? _settings;

	public TabInfoWrapper(TabInfo tab)
	{
		_tab = tab;
		TabModules = _tab.Modules.Cast<ModuleInfo>()
			.Select(module => new ModuleInfoWrapper(module))
			.Cast<IModuleInfo>()
			.ToArray();
	}

	public int? TabId => _tab.TabID;

	public int TabOrder => _tab.TabOrder;

	public int? ParentId => _tab.ParentId;

	public int? PortalId => _tab.PortalID;

	public string? CultureCode => _tab.CultureCode;

	public string TabName => _tab.TabName;

	public string? Title => _tab.Title;

	public string TabPath => _tab.TabPath;

	public bool IsVisible => _tab.IsVisible;

	public bool IsDeleted => _tab.IsDeleted;

	public bool DisableLink => _tab.DisableLink;

	public string? Description => _tab.Description;

	public string? SkinSrc => _tab.SkinSrc;

	public string? ContainerSrc => _tab.ContainerSrc;

	public string? KeyWords => _tab.KeyWords;

	public string? PageHeadText => _tab.PageHeadText;

	public string? Url => _tab.Url;

	public double SiteMapPriority => _tab.SiteMapPriority;

	public Dictionary<string, IPane> Panes => throw new NotSupportedException();

	public DateTime? CreatedOnDate => _tab.CreatedOnDate;

	public DateTime? LastModifiedOnDate => _tab.LastModifiedOnDate;

	public IEnumerable<IModuleInfo> TabModules { get; }

	public IReadOnlyDictionary<string, string> TabSettings => _settings ??= new HashtableDictionary<string, string>(_tab.TabSettings, string.Empty);

	public IEnumerable<ITabPermissionInfo> Permissions => Enumerable.Empty<ITabPermissionInfo>();
}
