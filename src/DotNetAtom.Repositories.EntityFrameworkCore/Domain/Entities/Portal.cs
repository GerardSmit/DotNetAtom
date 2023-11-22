using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class Portal : IEntity, ITimestamp
{
    public int Id { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int UserRegistration { get; set; }

    public int BannerAdvertising { get; set; }

    public int? AdministratorId { get; set; }

    public string? Currency { get; set; }

    public decimal HostFee { get; set; }

    public int HostSpace { get; set; }

    public int? AdministratorRoleId { get; set; }

    public int? RegisteredRoleId { get; set; }

    public Guid Guid { get; set; }

    public string? PaymentProcessor { get; set; }

    public string? ProcessorUserId { get; set; }

    public string? ProcessorPassword { get; set; }

    public int? SiteLogHistory { get; set; }

    public string DefaultLanguage { get; set; }

    public int TimezoneOffset { get; set; }

    public string HomeDirectory { get; set; }

    public int PageQuota { get; set; }

    public int UserQuota { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedOnDate { get; set; }

    public int? LastModifiedByUserId { get; set; }

    public DateTime? LastModifiedOnDate { get; set; }

    public int? PortalGroupId { get; set; }

    public User? Administrator { get; set; }

    public Role? AdministratorRole { get; set; }

    public Role? RegisteredRole { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public ICollection<PortalAlias> Aliases { get; set; } = new List<PortalAlias>();

    public ICollection<PortalLocalization> Localizations { get; set; } = new List<PortalLocalization>();

    public ICollection<PortalSetting> Settings { get; set; } = new List<PortalSetting>();

    public ICollection<Tab> Tabs { get; set; } = new List<Tab>();

    public ICollection<PortalDesktopModule> PortalDesktopModule { get; set; } = new List<PortalDesktopModule>();

    public ICollection<UserPortal> UserPortals { get; set; } = new List<UserPortal>();
}

public class PortalTypeConfiguration : IEntityTypeConfiguration<Portal>
{
    public void Configure(EntityTypeBuilder<Portal> builder)
    {
        builder.ToTable("Portals");
        builder.HasKey(pa => pa.Id);

        builder.Property(p => p.Id)
            .HasColumnName("PortalID");

        builder.Property(p => p.HostFee)
            .HasColumnType("money");

        builder.Property(p => p.Guid)
            .HasColumnName("GUID");

        builder.Property(p => p.CreatedByUserId)
            .HasColumnName("CreatedByUserID");

        builder.Property(p => p.LastModifiedByUserId)
            .HasColumnName("LastModifiedByUserID");

        builder.Property(p => p.PortalGroupId)
            .HasColumnName("PortalGroupID");

        builder.HasOne(p => p.Administrator)
            .WithMany()
            .HasForeignKey(p => p.AdministratorId)
            .HasPrincipalKey(u => u.Id);

        builder.HasOne(p => p.AdministratorRole)
            .WithMany()
            .HasForeignKey(p => p.AdministratorRoleId)
            .HasPrincipalKey(r => r.Id);

        builder.HasOne(p => p.RegisteredRole)
            .WithMany()
            .HasForeignKey(p => p.RegisteredRoleId)
            .HasPrincipalKey(r => r.Id);

        builder.HasMany(u => u.UserPortals)
            .WithOne(u => u.Portal)
            .HasForeignKey(u => u.PortalId)
            .HasPrincipalKey(u => u.Id)
            .IsRequired();
    }
}
