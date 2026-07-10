using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PVCAtolye.Application.Common.Security;
using PVCAtolye.Domain.Audit;
using PVCAtolye.Domain.Identity;
using PVCAtolye.Infrastructure.Identity;
using PVCAtolye.Infrastructure.Persistence;

namespace PVCAtolye.Infrastructure.Seed;

public sealed class DatabaseSeeder(AppDbContext dbContext, IPasswordHasher passwordHasher, IOptions<AdminSeedOptions> options)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedPermissionsAsync(cancellationToken);
        var ownerRole = await SeedOwnerRoleAsync(cancellationToken);
        await SeedAdminUserAsync(ownerRole, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPermissionsAsync(CancellationToken cancellationToken)
    {
        foreach (var code in PermissionCodes.All)
        {
            if (!await dbContext.Permissions.AnyAsync(permission => permission.Code == code, cancellationToken))
            {
                dbContext.Permissions.Add(new Permission(code, code));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Role> SeedOwnerRoleAsync(CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .Include(entity => entity.RolePermissions)
            .SingleOrDefaultAsync(entity => entity.NormalizedName == "ISLETME SAHIBI", cancellationToken);

        if (role is null)
        {
            role = new Role("Isletme Sahibi", "Tum sistem yetkilerine sahip rol.");
            dbContext.Roles.Add(role);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var permissions = await dbContext.Permissions.ToListAsync(cancellationToken);
        foreach (var permission in permissions)
        {
            role.AddPermission(permission);
        }

        return role;
    }

    private async Task SeedAdminUserAsync(Role ownerRole, CancellationToken cancellationToken)
    {
        var adminOptions = options.Value;
        var normalizedUsername = adminOptions.Username.Trim().ToUpperInvariant();
        var admin = await dbContext.Users
            .Include(entity => entity.UserRoles)
            .SingleOrDefaultAsync(entity => entity.NormalizedUsername == normalizedUsername, cancellationToken);

        if (admin is not null)
        {
            return;
        }

        admin = new User(adminOptions.Username, adminOptions.DisplayName, passwordHasher.HashPassword(adminOptions.Password), mustChangePassword: true);
        admin.AddRole(ownerRole);
        dbContext.Users.Add(admin);
        dbContext.AuditLogs.Add(new AuditLog(null, "Seed.AdminCreated", nameof(User), admin.Id.ToString(), DateTimeOffset.UtcNow, "Varsayilan admin kullanicisi olusturuldu."));
    }
}
