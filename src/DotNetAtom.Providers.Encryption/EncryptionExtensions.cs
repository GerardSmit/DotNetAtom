using DotNetAtom.Options;
using DotNetAtom.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAtom;

public static class EncryptionExtensions
{
    public static AtomBuilder AddTripleDES(this AtomBuilder builder, string encryptionKey)
    {
        builder.Services.Configure<MachineKeyOptions>(options =>
        {
            options.DecryptionKey = encryptionKey;
        });
        builder.Services.AddSingleton<IPasswordHasher, TripleDESPasswordHasher>();
        return builder;
    }
}