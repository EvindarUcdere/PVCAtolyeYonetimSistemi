using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PVCAtolye.Application.Common.Security;
using PVCAtolye.Application.Identity;

namespace PVCAtolye.Infrastructure.Identity;

public sealed class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    public (string Token, DateTimeOffset ExpiresAt) CreateAccessToken(CurrentUserResponse user)
    {
        var jwtOptions = options.Value;
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.AccessTokenMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(AppClaimTypes.UserId, user.Id.ToString()),
            new(AppClaimTypes.Username, user.Username),
            new(ClaimTypes.Name, user.DisplayName),
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(user.Permissions.Select(permission => new Claim(AppClaimTypes.Permission, permission)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
            claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
