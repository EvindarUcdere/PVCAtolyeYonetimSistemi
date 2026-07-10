namespace PVCAtolye.Application.Identity;

public sealed record CurrentUserResponse(
    Guid Id,
    string Username,
    string DisplayName,
    bool MustChangePassword,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions);
