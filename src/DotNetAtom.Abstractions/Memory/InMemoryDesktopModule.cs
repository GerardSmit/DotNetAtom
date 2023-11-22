using System;
using DotNetAtom.Entities;

namespace DotNetAtom.Memory;

public class InMemoryDesktopModule : IDesktopModuleInfo
{
	public int DesktopModuleId { get; set; } = -1;
	public string FriendlyName { get; set; } = "";
	public string? Description { get; set; }
	public string? Version { get; set; }
	public bool IsPremium { get; set; }
	public bool IsAdmin { get; set; }
	public string? BusinessControllerClass { get; set; }
	public string FolderName { get; set; } = "";
	public string ModuleName { get; set; } = "";
	public int SupportedFeatures { get; set; }
	public string? CompatibleVersions { get; set; }
	public string? Dependencies { get; set; }
	public string? Permissions { get; set; }
	public int PackageId { get; set; }
	public DateTime? CreatedOnDate { get; set; }
	public DateTime? LastModifiedOnDate { get; set; }
}