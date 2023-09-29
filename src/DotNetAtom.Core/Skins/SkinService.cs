using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Ben.Collections.Specialized;
using DotNetAtom.Portals;

namespace DotNetAtom.Skins;

public class SkinService : ISkinService
{
    private readonly IInternPool _internPool = new InternPool();
    private readonly IAtomGlobals _globals;

    public SkinService(IAtomGlobals globals)
    {
        _globals = globals;
    }

    [return: NotNullIfNotNull(nameof(skinSrc))]
    public string? GetSkinSrc(string? skinSrc, IPortalInfo portalInfo)
    {
        if (skinSrc == null)
        {
            return null;
        }

        var spanSkinSrc = skinSrc.AsSpan();

        if (spanSkinSrc.Length < 3 || spanSkinSrc[0] != '[' || spanSkinSrc[2] != ']')
        {
            return skinSrc;
        }

        var value = spanSkinSrc[1] switch
        {
            'g' or 'G' => _globals.HostPath,
            's' or 'S' => portalInfo.HomeSystemDirectory,
            'l' or 'L' => portalInfo.HomeDirectory,
            _ => null
        };

        if (value == null)
        {
            return skinSrc;
        }

        var length = value.Length + spanSkinSrc.Length - 3;
        var span = length <= 256 ? stackalloc char[length] : new char[length];

        value.AsSpan().CopyTo(span);
        spanSkinSrc.Slice(3).CopyTo(span.Slice(value.Length));
        CorrectPathSeparators(span);

        lock (_internPool)
        {
            return _internPool.Intern(span);
        }
    }

    private static void CorrectPathSeparators(Span<char> span)
    {
        var separator = Path.DirectorySeparatorChar;
        var otherSeparator = separator == '/' ? '\\' : '/';
        var index = span.IndexOf(otherSeparator);

        while (index >= 0)
        {
            span[index] = separator;
            span = span.Slice(index + 1);
            index = span.IndexOf(otherSeparator);
        }
    }
}
