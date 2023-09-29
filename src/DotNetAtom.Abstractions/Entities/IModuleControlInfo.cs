using System;

namespace DotNetAtom.Entities;

public interface IModuleControlInfo
{
    int ModuleControlId { get; }

    string? ControlKey { get; }

    string? ControlTitle { get; }

    string? ControlSrc { get; }

    string? IconFile { get; }

    string? HelpUrl { get; }

    bool SupportsPartialRendering { get; }

    bool SupportsPopUps { get; }

    DateTime? CreatedOnDate { get; }

    DateTime? LastModifiedOnDate { get; }
}
