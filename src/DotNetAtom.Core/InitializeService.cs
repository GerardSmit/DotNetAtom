using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Modules;
using DotNetAtom.Portals;
using DotNetAtom.Tabs;
using Microsoft.Extensions.Hosting;

namespace DotNetAtom;

internal class InitializeService : IHostedService
{
    private readonly IPortalService _portalService;
    private readonly ITabService _tabService;
    private readonly ITabRouter _tabRouter;
    private readonly IModuleService _moduleService;

    public InitializeService(IPortalService portalService, ITabService tabService, ITabRouter tabRouter, IModuleService moduleService)
    {
        _portalService = portalService;
        _tabService = tabService;
        _tabRouter = tabRouter;
        _moduleService = moduleService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _portalService.LoadAsync();
        await _tabService.LoadAsync();
        await _tabRouter.LoadAsync();
        await _moduleService.LoadAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
