using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PVCAtolye.Application.Common.Models;
using PVCAtolye.Application.Common.Security;
using PVCAtolye.Application.Identity;

namespace PVCAtolye.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        if (!response.Success)
        {
            return Unauthorized(response);
        }

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<CurrentUserResponse>>> Me(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(AppClaimTypes.UserId);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(ApiResponse.Fail<CurrentUserResponse>("Gecersiz oturum."));
        }

        var response = await authService.GetCurrentUserAsync(userId, cancellationToken);
        return response.Success ? Ok(response) : Unauthorized(response);
    }
}
