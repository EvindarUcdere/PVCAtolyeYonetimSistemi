using PVCAtolye.Application.Identity;

namespace PVCAtolye.Infrastructure.Identity;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAt) CreateAccessToken(CurrentUserResponse user);
}
