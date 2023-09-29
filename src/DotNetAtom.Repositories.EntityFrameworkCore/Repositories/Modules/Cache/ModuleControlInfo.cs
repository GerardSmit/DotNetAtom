using System;
using DotNetAtom.Entities;

namespace DotNetAtom.Tabs.Cache;

public class ModuleControlInfo : IModuleControlInfo
{
    public ModuleControlInfo(ModuleControl moduleControl)
    {
        ModuleControlId = moduleControl.Id;
        ControlKey = moduleControl.ControlKey;
        ControlTitle = moduleControl.ControlTitle;
        ControlSrc = moduleControl.ControlSrc;
        IconFile = moduleControl.IconFile;
        HelpUrl = moduleControl.HelpUrl;
        SupportsPartialRendering = moduleControl.SupportsPartialRendering;
        SupportsPopUps = moduleControl.SupportsPopUps;
        CreatedOnDate = moduleControl.CreatedOnDate;
        LastModifiedOnDate = moduleControl.LastModifiedOnDate;
    }

    public int ModuleControlId { get; }

    public string? ControlKey { get; }

    public string? ControlTitle { get; }

    public string? ControlSrc { get; }

    public string? IconFile { get; }

    public string? HelpUrl { get; }

    public bool SupportsPartialRendering { get; }

    public bool SupportsPopUps { get; }

    public DateTime? CreatedOnDate { get; }

    public DateTime? LastModifiedOnDate { get; }
}