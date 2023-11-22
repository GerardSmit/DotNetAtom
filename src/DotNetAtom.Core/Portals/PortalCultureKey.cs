using System;
using System.Collections.Generic;

namespace DotNetAtom.Portals;

internal readonly record struct PortalCultureKey(int PortalId, string? CultureCode)
{
    public static readonly IEqualityComparer<PortalCultureKey> OrdinalIgnoreCase = new PortalKeyEqualityComparer(StringComparer.OrdinalIgnoreCase);

    private class PortalKeyEqualityComparer(IEqualityComparer<string?> stringComparer)
        : IEqualityComparer<PortalCultureKey>
    {
        public bool Equals(PortalCultureKey x, PortalCultureKey y)
        {
            return x.PortalId == y.PortalId && stringComparer.Equals(x.CultureCode, y.CultureCode);
        }

        public int GetHashCode(PortalCultureKey obj)
        {
            if (obj.CultureCode is null)
            {
                return obj.PortalId;
            }

#if NET
            return HashCode.Combine(obj.PortalId, stringComparer.GetHashCode(obj.CultureCode));
#else
            unchecked
            {
                var hash = 17;
                hash = hash * 31 + obj.PortalId.GetHashCode();
                hash = hash * 31 + stringComparer.GetHashCode(obj.CultureCode);
                return hash;
            }
#endif
        }
    }
}
