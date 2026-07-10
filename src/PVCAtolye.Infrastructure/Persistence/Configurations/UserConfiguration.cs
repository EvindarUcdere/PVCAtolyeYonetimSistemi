using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCAtolye.Domain.Identity;

namespace PVCAtolye.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Username).HasMaxLength(64).IsRequired();
        builder.Property(user => user.NormalizedUsername).HasMaxLength(64).IsRequired();
        builder.Property(user => user.DisplayName).HasMaxLength(128).IsRequired();
        builder.Property(user => user.PasswordHash).HasMaxLength(512).IsRequired();
        builder.HasIndex(user => user.NormalizedUsername).IsUnique();
        builder.Navigation(user => user.UserRoles).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(user => user.RefreshTokens).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
