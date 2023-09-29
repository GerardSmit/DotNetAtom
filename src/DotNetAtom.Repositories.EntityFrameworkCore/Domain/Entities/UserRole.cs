using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetAtom.Entities;

public class UserRole : IEntity
{
    public int Id { get; set; }

    public User User { get; set; }

    public int UserId { get; set; }

    public Role Role { get; set; }

    public int RoleId { get; set; }
}

public class UserRoleTypeConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(pa => pa.Id);

        builder.Property(p => p.Id)
            .HasColumnName("UserRoleID");

        builder.Property(p => p.UserId)
            .HasColumnName("UserID");

        builder.Property(p => p.RoleId)
            .HasColumnName("RoleID");
    }
}
