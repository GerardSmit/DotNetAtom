using WebFormsCore.UI;

namespace DotNetAtom.Modules;

/// <summary>IModuleControl provides a common Interface for Module Controls.</summary>
public interface IModuleControl
{
    /// <summary>Gets the control.</summary>
    Control Control { get; }

    /// <summary>Gets the control path.</summary>
    string ControlPath { get; }

    /// <summary>Gets the control name.</summary>
    string ControlName { get; }

    /// <summary>Gets the module context.</summary>
    ModuleInstanceContext ModuleContext { get; }

    /// <summary>Gets or sets the local resource localization file for the control.</summary>
    string LocalResourceFile { get; set; }
}