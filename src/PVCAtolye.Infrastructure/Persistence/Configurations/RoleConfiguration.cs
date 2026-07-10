using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCAtolye.Domain.Identity;

namespace PVCAtolye.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(role => role.Id);
        builder.Property(role => role.Name).HasMaxLength(64).IsRequired();
        builder.Property(role => role.NormalizedName).HasMaxLength(64).IsRequired();
        builder.Property(role => role.Description).HasMaxLength(256).IsRequired();
        builder.HasIndex(role => role.NormalizedName).IsUnique();
        builder.Navigation(role => role.UserRoles).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(role => role.RolePermissions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
