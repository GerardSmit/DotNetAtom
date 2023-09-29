namespace DotNetAtom.Framework;

internal sealed class AtomFeature : IAtomFeature
{
    public IAtomContext AtomContext { get; set; } = null!;
}
