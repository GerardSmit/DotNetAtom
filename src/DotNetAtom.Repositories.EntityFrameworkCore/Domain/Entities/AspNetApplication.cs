using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class AspNetApplication
{
    public Guid Id { get; set; }

    public string ApplicationName { get; set; }

    public string LoweredApplicationName { get; set; }
}

public class AspNetApplicationTypeConfiguration : IEntityTypeConfiguration<AspNetApplication>
{
    public void Configure(EntityTypeBuilder<AspNetApplication> builder)
    {
        builder.ToTable("aspnet_Applications");
        builder.HasKey(u => u.Id);

        builder.Property(p => p.Id)
            .HasColumnName("ApplicationId");
    }
}
