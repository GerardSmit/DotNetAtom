using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetAtom.Database;

internal class DbContextFactoryWrapper<TContext, TTarget> : IDbContextFactory<TTarget>
    where TContext : TTarget
    where TTarget : DbContext
{
    private readonly IDbContextFactory<TContext> _factory;

    public DbContextFactoryWrapper(IDbContextFactory<TContext> factory)
    {
        _factory = factory;
    }

    public TTarget CreateDbContext()
    {
        return _factory.CreateDbContext();
    }

    public async Task<TTarget> CreateDbContextAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await _factory.CreateDbContextAsync(cancellationToken);
    }
}
