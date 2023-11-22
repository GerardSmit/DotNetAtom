using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Memory;
using DotNetAtom.Portals;

namespace DotNetAtom.Providers;

/// <summary>In-memory tab provider for the login page of each portal that doesn't have a login tab.</summary>
public class LoginTabProvider(IPortalService portalService) : ITabProvider
{
    public ValueTask<IReadOnlyList<ITabInfo>> GetTabs()
    {
        var tabs = portalService.Portals
            .Where(portal => portal.LoginTabId == -1)
            .Select(portal => new InMemoryTabInfo
            {
                PortalId = portal.PortalId,
                TabName = "Login",
                TabPath = "//Login",
                TabModules =
                {
                    new InMemoryModuleInfo
                    {
                        ModuleDefinition = new InMemoryModuleDefinition
                        {
                            Controls =
                            {
                                [null] = new InMemoryModuleControlInfo
                                {
                                    ControlSrc = "DesktopModules/Admin/Authentication/Login.ascx"
                                }
                            }
                        }
                    }
                },
                TabSettings =
                {
                    ["_IsLoginTab"] = "true"
                }
            })
            .ToList();

        return new ValueTask<IReadOnlyList<ITabInfo>>(tabs);
    }
}
