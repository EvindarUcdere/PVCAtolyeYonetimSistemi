namespace PVCAtolye.Application.Identity;

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    CurrentUserResponse User);
