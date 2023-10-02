using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DotNetAtom;

public readonly record struct StringKey(string? Value)
{
    public override string ToString()
    {
        return Value ?? "";
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue => !string.IsNullOrEmpty(Value);

    public static implicit operator StringKey(string? cultureCode) => new(cultureCode);

    public static implicit operator string?(StringKey cultureCode) => cultureCode.Value;
}

public sealed class StringKeyComparer : IComparer<StringKey>, IEqualityComparer<StringKey>
{
    public static readonly IEqualityComparer<StringKey> Ordinal = new StringKeyComparer(StringComparer.Ordinal);
    public static readonly IEqualityComparer<StringKey> OrdinalIgnoreCase = new StringKeyComparer(StringComparer.OrdinalIgnoreCase);

    private readonly StringComparer _comparer;

    public StringKeyComparer(StringComparer comparer)
    {
        _comparer = comparer;
    }

    public int Compare(StringKey x, StringKey y)
    {
        return _comparer.Compare(x.Value, y.Value);
    }

    public bool Equals(StringKey x, StringKey y)
    {
        return _comparer.Equals(x.Value, y.Value);
    }

    public int GetHashCode(StringKey obj)
    {
        return _comparer.GetHashCode(obj.Value ?? string.Empty);
    }
}
