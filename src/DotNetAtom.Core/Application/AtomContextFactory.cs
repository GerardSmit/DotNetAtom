using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DotNetAtom.Entities;
using DotNetAtom.Portals;
using Microsoft.Extensions.ObjectPool;

namespace DotNetAtom.Application;

internal class AtomContextFactory : IAtomContextFactory
{
    private Guid? _applicationId;
    private readonly ObjectPool<AtomContext> _contextPool;
    private readonly IApplicationRepository _applicationRepository;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public AtomContextFactory(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
        _contextPool = new DefaultObjectPool<AtomContext>(new AtomContextPooledObjectPolicy());
    }

    public ValueTask<IAtomContext> CreateAsync(IPortalInfo portal, ITabInfo? tab = null)
    {
        return _applicationId.HasValue
            ? CreateContextAsync(portal, tab)
            : InitializeAndCreateContextAsync(portal, tab);
    }

    public void Return(IAtomContext context)
    {
        if (context is not AtomContext atomContext)
        {
            return;
        }

        _contextPool.Return(atomContext);
    }

    private async ValueTask<IAtomContext> InitializeAndCreateContextAsync(IPortalInfo portal, ITabInfo? tab)
    {
        await InitializeAsync();
        return await CreateContextAsync(portal, tab);
    }

    private async Task InitializeAsync()
    {
        await _lock.WaitAsync();

        try
        {
            var guid = await _applicationRepository.GetApplicationId();

            _applicationId = guid ?? throw new InvalidOperationException("Application ID not found");
        }
        finally
        {
            _lock.Release();
        }
    }

    private ValueTask<IAtomContext> CreateContextAsync(IPortalInfo portal, ITabInfo? tab)
    {
        Debug.Assert(_applicationId.HasValue);

        var context = _contextPool.Get();
        context.Factory = this;
        context.ApplicationId = _applicationId!.Value;
        context.Initialize(portal, tab);
        return new ValueTask<IAtomContext>(context);
    }

    private class AtomContextPooledObjectPolicy : IPooledObjectPolicy<AtomContext>
    {
        public AtomContext Create()
        {
            return new AtomContext();
        }

        public bool Return(AtomContext obj)
        {
            obj.Clear();
            return true;
        }
    }
}
