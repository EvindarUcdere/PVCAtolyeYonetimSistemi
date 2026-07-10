using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Identity;

public sealed class Permission : BaseEntity
{
    private readonly List<RolePermission> _rolePermissions = [];

    private Permission()
    {
    }

    public Permission(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions;
}
