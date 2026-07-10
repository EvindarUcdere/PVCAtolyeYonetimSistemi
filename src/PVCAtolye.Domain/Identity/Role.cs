using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Identity;

public sealed class Role : AuditableEntity
{
    private readonly List<UserRole> _userRoles = [];
    private readonly List<RolePermission> _rolePermissions = [];

    private Role()
    {
    }

    public Role(string name, string description)
    {
        Name = name;
        NormalizedName = name.Trim().ToUpperInvariant();
        Description = description;
        IsSystemRole = true;
    }

    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles;
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions;

    public void AddPermission(Permission permission)
    {
        if (_rolePermissions.Any(rolePermission => rolePermission.PermissionId == permission.Id))
        {
            return;
        }

        _rolePermissions.Add(new RolePermission(Id, permission.Id));
    }
}
