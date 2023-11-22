using System;
using DotNetAtom.Entities;

namespace DotNetAtom.Memory;

public class InMemoryModuleControlInfo : IModuleControlInfo
{
	public int ModuleControlId { get; set; } = -1;
	public string? ControlKey { get; set; }
	public string? ControlTitle { get; set; }
	public string? ControlSrc { get; set; }
	public string? IconFile { get; set; }
	public string? HelpUrl { get; set; }
	public bool SupportsPartialRendering { get; set; }
	public bool SupportsPopUps { get; set; }
	public DateTime? CreatedOnDate { get; set; }
	public DateTime? LastModifiedOnDate { get; set; }
}