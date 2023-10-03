using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Memory;

namespace DotNetAtom.Providers;

/// <summary>
/// In-memory tab provider for the login page.
/// </summary>
public class LoginTabProvider : ITabProvider
{
    public ValueTask<IReadOnlyList<ITabInfo>> GetTabs()
    {
        return new ValueTask<IReadOnlyList<ITabInfo>>(
            new[]
            {
                new InMemoryTabInfo
                {
                    TabName = "Login",
                    TabPath = "//Login",
                    TabModules =
                    {
                        new InMemoryModuleInfo
                        {
                            ModuleDefinitionFriendlyName = "Account Login"
                        }
                    }
                }
            }
        );
    }
}
