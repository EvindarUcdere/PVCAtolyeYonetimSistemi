using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCAtolye.Domain.Identity;

namespace PVCAtolye.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(permission => permission.Id);
        builder.Property(permission => permission.Code).HasMaxLength(128).IsRequired();
        builder.Property(permission => permission.Description).HasMaxLength(256).IsRequired();
        builder.HasIndex(permission => permission.Code).IsUnique();
        builder.Navigation(permission => permission.RolePermissions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
