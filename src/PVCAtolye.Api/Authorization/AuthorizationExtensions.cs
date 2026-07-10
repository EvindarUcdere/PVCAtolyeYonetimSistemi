using Microsoft.AspNetCore.Authorization;
using PVCAtolye.Application.Common.Security;

namespace PVCAtolye.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorization(options =>
        {
            foreach (var permission in PermissionCodes.All)
            {
                options.AddPolicy(permission, policy => policy.Requirements.Add(new PermissionRequirement(permission)));
            }
        });

        return services;
    }
}
