namespace PVCAtolye.Application.Common.Security;

public static class PermissionCodes
{
    public const string UsersRead = "Users.Read";
    public const string UsersManage = "Users.Manage";
    public const string RolesRead = "Roles.Read";
    public const string RolesManage = "Roles.Manage";
    public const string SettingsRead = "Settings.Read";
    public const string SettingsManage = "Settings.Manage";
    public const string AuditRead = "Audit.Read";

    public static readonly IReadOnlyCollection<string> All =
    [
        UsersRead,
        UsersManage,
        RolesRead,
        RolesManage,
        SettingsRead,
        SettingsManage,
        AuditRead,
    ];
}
