using System.Threading.Tasks;
using DotNetAtom.Framework;
using DotNetAtom.Portals;
using DotNetAtom.Tabs;
using HttpStack;
using Microsoft.Extensions.ObjectPool;

namespace DotNetAtom.Middlewares;

internal sealed class AtomMiddleware : IMiddleware
{
    private readonly IAtomContextFactory _contextFactory;
    private readonly ITabRouter _tabRouter;
    private readonly ITabService _tabService;
    private readonly IPortalService _portalService;
    private readonly ObjectPool<AtomFeature> _atomFeaturePool;

    public AtomMiddleware(IAtomContextFactory contextFactory, ITabRouter tabRouter, ITabService tabService, IPortalService portalService)
    {
        _contextFactory = contextFactory;
        _tabRouter = tabRouter;
        _tabService = tabService;
        _portalService = portalService;
        _atomFeaturePool = new DefaultObjectPoolProvider().Create(new AtomFeaturePooledObjectPolicy());
    }

    public async Task Invoke(IHttpContext context, MiddlewareDelegate next)
    {
        var portalId = 0;
        var culture = _portalService.GetDefaultCulture(portalId);

        ITabRoute? match;

        if (context.Request.Path.Equals("/") &&
            _tabRouter.TryGetHomeTab(portalId, culture, out var homeTab))
        {
            match = homeTab;
        }
        else if (!_tabRouter.Match(portalId, culture, context.Request, out match))
        {
            await next(context);
            return;
        }

        var tab = match.Tab;
        var portal = _portalService.GetPortal(portalId, tab.CultureCode);
        using var atomContext = await _contextFactory.CreateAsync(portal, tab);

        var atomFeature = _atomFeaturePool.Get();

        try
        {
            atomFeature.AtomContext = atomContext;
            context.Features.Set<IAtomFeature>(atomFeature);
            await next(context);
        }
        finally
        {
            _atomFeaturePool.Return(atomFeature);
        }
    }

    private sealed class AtomFeaturePooledObjectPolicy : IPooledObjectPolicy<AtomFeature>
    {
        public AtomFeature Create()
        {
            return new AtomFeature();
        }

        public bool Return(AtomFeature obj)
        {
            obj.AtomContext = null!;
            return true;
        }
    }
}
