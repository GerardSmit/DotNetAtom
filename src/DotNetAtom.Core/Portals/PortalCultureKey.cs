using System;
using System.Collections.Generic;

namespace DotNetAtom.Portals;

internal readonly record struct PortalCultureKey(int PortalId, string? CultureCode)
{
    public static readonly IEqualityComparer<PortalCultureKey> IgnoreCaseComparer = new PortalKeyEqualityComparer(StringComparer.OrdinalIgnoreCase);

    private class PortalKeyEqualityComparer : IEqualityComparer<PortalCultureKey>
    {
        private readonly IEqualityComparer<string?> _stringComparer;

        public PortalKeyEqualityComparer(IEqualityComparer<string?> stringComparer)
        {
            _stringComparer = stringComparer;
        }

        public bool Equals(PortalCultureKey x, PortalCultureKey y)
        {

            return x.PortalId == y.PortalId && _stringComparer.Equals(x.CultureCode, y.CultureCode);
        }

        public int GetHashCode(PortalCultureKey obj)
        {
            return obj.CultureCode is not null
                ? HashCode.Combine(obj.PortalId, _stringComparer.GetHashCode(obj.CultureCode))
                : obj.PortalId;
        }
    }
}
