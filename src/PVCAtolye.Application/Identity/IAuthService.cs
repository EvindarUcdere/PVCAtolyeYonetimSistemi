using PVCAtolye.Application.Common.Models;

namespace PVCAtolye.Application.Identity;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);
}
