using Microsoft.AspNetCore.Mvc;
using PVCAtolye.Application.Common.Models;

namespace PVCAtolye.Api.Controllers;

[ApiController]
[Route("api/system")]
public sealed class SystemController : ControllerBase
{
    [HttpGet("info")]
    public ActionResult<ApiResponse<SystemInfoResponse>> GetInfo()
    {
        var response = new SystemInfoResponse("PVC Atolye Yonetim Sistemi", "0.1.0", "Faz 1");
        return Ok(ApiResponse.Ok(response));
    }
}

public sealed record SystemInfoResponse(string ProductName, string Version, string Phase);

