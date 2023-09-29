using System.Threading.Tasks;
using DotNetAtom.Framework;
using HttpStack;
using WebFormsCore;

namespace DotNetAtom.Middlewares;

internal sealed class AtomPageMiddleware : IMiddleware
{
    public async Task Invoke(IHttpContext context, MiddlewareDelegate next)
    {
        var settings = context.Features.Get<IAtomFeature>()?.AtomContext.PortalSettings;

        if (settings is not { ActiveTab.IsDeleted: false })
        {
            await next(context);
        }

        await context.ExecutePageAsync<Default>();
    }
}
