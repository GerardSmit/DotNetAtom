using DotNetAtom.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom;

internal static class AtomDbContextColumnFixer
{
    public static void OnColumnIgnore(IMutableProperty property, EntityTypeBuilder builder)
    {
        // Before DNN7 the primary key was a composite key.
        if (builder.Metadata.ClrType == typeof(PortalSetting) && property.Name == nameof(PortalSetting.PortalSettingId))
        {
            builder.HasKey(nameof(PortalSetting.PortalId), nameof(PortalSetting.SettingName), nameof(PortalSetting.CultureCode));
            builder.Property(nameof(PortalSetting.CultureCode)).IsRequired();
        }
    }
}
