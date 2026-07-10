using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Identity;

public sealed class User : AuditableEntity
{
    private readonly List<UserRole> _userRoles = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    private User()
    {
    }

    public User(string username, string displayName, string passwordHash, bool mustChangePassword)
    {
        Username = username;
        NormalizedUsername = username.Trim().ToUpperInvariant();
        DisplayName = displayName;
        PasswordHash = passwordHash;
        MustChangePassword = mustChangePassword;
        IsActive = true;
    }

    public string Username { get; private set; } = string.Empty;
    public string NormalizedUsername { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public bool MustChangePassword { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles;
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    public void AddRole(Role role)
    {
        if (_userRoles.Any(userRole => userRole.RoleId == role.Id))
        {
            return;
        }

        _userRoles.Add(new UserRole(Id, role.Id));
    }

    public void RecordLogin(DateTimeOffset loginTime)
    {
        LastLoginAt = loginTime;
    }
}
