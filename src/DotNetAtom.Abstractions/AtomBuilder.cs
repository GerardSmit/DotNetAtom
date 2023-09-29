using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom;

public sealed class AtomBuilder
{
    public AtomBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public Dictionary<object, object?> Items { get; } = new Dictionary<object, object?>();

    public IServiceCollection Services { get; set; }
}
