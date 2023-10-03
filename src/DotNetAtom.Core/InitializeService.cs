using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Modules;
using DotNetAtom.Portals;
using DotNetAtom.Security;
using DotNetAtom.Tabs;
using Microsoft.Extensions.Hosting;

namespace DotNetAtom;

internal class InitializeService(
    IPortalService portalService,
    ITabService tabService,
    ITabRouter tabRouter,
    IModuleService moduleService,
    IAuthenticationService authenticationService
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await portalService.LoadAsync();
        await tabService.LoadAsync();
        await tabRouter.LoadAsync();
        await moduleService.LoadAsync();
        await authenticationService.LoadAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}