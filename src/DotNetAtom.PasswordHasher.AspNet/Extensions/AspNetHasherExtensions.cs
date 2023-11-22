using System;
using DotNetAtom.Options;
using DotNetAtom.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom;

public static class AspNetHasherExtensions
{
	public static AtomBuilder AddAspNetPasswordHasher(this AtomBuilder builder, Action<MachineOptions>? configure = null)
	{
		builder.Services.AddSingleton<IPasswordHasher, HashedAspNetPasswordHasher>();
		builder.Services.AddSingleton<IPasswordHasher, EncryptedAspNetPasswordHasher>();

		if (configure is not null)
		{
			builder.Services.AddOptions<MachineOptions>().Configure(configure);
		}

		return builder;
	}
}
