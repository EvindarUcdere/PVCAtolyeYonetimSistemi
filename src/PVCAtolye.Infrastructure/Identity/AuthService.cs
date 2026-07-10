using Microsoft.EntityFrameworkCore;
using PVCAtolye.Application.Common.Models;
using PVCAtolye.Application.Identity;
using PVCAtolye.Domain.Audit;
using PVCAtolye.Infrastructure.Persistence;

namespace PVCAtolye.Infrastructure.Identity;

public sealed class AuthService(AppDbContext dbContext, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedUsername = request.Username.Trim().ToUpperInvariant();
        var user = await dbContext.Users
            .Include(entity => entity.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .ThenInclude(role => role.RolePermissions)
            .ThenInclude(rolePermission => rolePermission.Permission)
            .SingleOrDefaultAsync(entity => entity.NormalizedUsername == normalizedUsername, cancellationToken);

        if (user is null || !user.IsActive || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return ApiResponse.Fail<LoginResponse>("Kullanici adi veya sifre hatali.");
        }

        user.RecordLogin(DateTimeOffset.UtcNow);
        dbContext.AuditLogs.Add(new AuditLog(user.Id, "Auth.Login", nameof(user), user.Id.ToString(), DateTimeOffset.UtcNow, "Kullanici giris yapti."));
        await dbContext.SaveChangesAsync(cancellationToken);

        var currentUser = MapCurrentUser(user);
        var token = jwtTokenService.CreateAccessToken(currentUser);
        return ApiResponse.Ok(new LoginResponse(token.Token, token.ExpiresAt, currentUser));
    }

    public async Task<ApiResponse<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(entity => entity.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .ThenInclude(role => role.RolePermissions)
            .ThenInclude(rolePermission => rolePermission.Permission)
            .SingleOrDefaultAsync(entity => entity.Id == userId && entity.IsActive, cancellationToken);

        if (user is null)
        {
            return ApiResponse.Fail<CurrentUserResponse>("Oturum kullanicisi bulunamadi.");
        }

        return ApiResponse.Ok(MapCurrentUser(user));
    }

    private static CurrentUserResponse MapCurrentUser(Domain.Identity.User user)
    {
        var roles = user.UserRoles.Select(userRole => userRole.Role.Name).OrderBy(role => role).ToArray();
        var permissions = user.UserRoles
            .SelectMany(userRole => userRole.Role.RolePermissions)
            .Select(rolePermission => rolePermission.Permission.Code)
            .Distinct()
            .OrderBy(permission => permission)
            .ToArray();

        return new CurrentUserResponse(user.Id, user.Username, user.DisplayName, user.MustChangePassword, roles, permissions);
    }
}
