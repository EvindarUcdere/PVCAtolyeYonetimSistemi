using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PVCAtolye.Application.Common.Models;
using PVCAtolye.Application.Common.Security;
using PVCAtolye.Application.Settings;

namespace PVCAtolye.Api.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize(Policy = PermissionCodes.SettingsRead)]
public sealed class SettingsController(
    ISettingsService settingsService,
    IValidator<CompanyProfileRequest> companyProfileValidator,
    IValidator<WarehouseRequest> warehouseValidator,
    IValidator<DefinitionItemRequest> definitionValidator,
    IValidator<NumberSequenceRequest> numberSequenceValidator) : ControllerBase
{
    [HttpGet("company-profile")]
    public async Task<ActionResult<ApiResponse<CompanyProfileResponse>>> GetCompanyProfile(CancellationToken cancellationToken)
    {
        var response = await settingsService.GetCompanyProfileAsync(cancellationToken);
        return ToActionResult(response);
    }

    [HttpPut("company-profile")]
    [Authorize(Policy = PermissionCodes.SettingsManage)]
    public async Task<ActionResult<ApiResponse<CompanyProfileResponse>>> UpdateCompanyProfile(CompanyProfileRequest request, CancellationToken cancellationToken)
    {
        var validation = await companyProfileValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse<CompanyProfileResponse>(validation));
        }

        var response = await settingsService.UpdateCompanyProfileAsync(request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpGet("warehouses")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<WarehouseResponse>>>> GetWarehouses(CancellationToken cancellationToken)
    {
        var response = await settingsService.GetWarehousesAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPost("warehouses")]
    [Authorize(Policy = PermissionCodes.SettingsManage)]
    public async Task<ActionResult<ApiResponse<WarehouseResponse>>> CreateWarehouse(WarehouseRequest request, CancellationToken cancellationToken)
    {
        var validation = await warehouseValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse<WarehouseResponse>(validation));
        }

        var response = await settingsService.CreateWarehouseAsync(request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpPut("warehouses/{id:guid}")]
    [Authorize(Policy = PermissionCodes.SettingsManage)]
    public async Task<ActionResult<ApiResponse<WarehouseResponse>>> UpdateWarehouse(Guid id, WarehouseRequest request, CancellationToken cancellationToken)
    {
        var validation = await warehouseValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse<WarehouseResponse>(validation));
        }

        var response = await settingsService.UpdateWarehouseAsync(id, request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpGet("definitions/{definitionType}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<DefinitionItemResponse>>>> GetDefinitions(string definitionType, CancellationToken cancellationToken)
    {
        var response = await settingsService.GetDefinitionsAsync(definitionType, cancellationToken);
        return ToActionResult(response);
    }

    [HttpPost("definitions/{definitionType}")]
    [Authorize(Policy = PermissionCodes.SettingsManage)]
    public async Task<ActionResult<ApiResponse<DefinitionItemResponse>>> CreateDefinition(string definitionType, DefinitionItemRequest request, CancellationToken cancellationToken)
    {
        var validation = await definitionValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse<DefinitionItemResponse>(validation));
        }

        var response = await settingsService.CreateDefinitionAsync(definitionType, request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpPut("definitions/{definitionType}/{id:guid}")]
    [Authorize(Policy = PermissionCodes.SettingsManage)]
    public async Task<ActionResult<ApiResponse<DefinitionItemResponse>>> UpdateDefinition(string definitionType, Guid id, DefinitionItemRequest request, CancellationToken cancellationToken)
    {
        var validation = await definitionValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse<DefinitionItemResponse>(validation));
        }

        var response = await settingsService.UpdateDefinitionAsync(definitionType, id, request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpGet("number-sequences")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<NumberSequenceResponse>>>> GetNumberSequences(CancellationToken cancellationToken)
    {
        var response = await settingsService.GetNumberSequencesAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPut("number-sequences/{documentType}")]
    [Authorize(Policy = PermissionCodes.SettingsManage)]
    public async Task<ActionResult<ApiResponse<NumberSequenceResponse>>> UpdateNumberSequence(string documentType, NumberSequenceRequest request, CancellationToken cancellationToken)
    {
        var validation = await numberSequenceValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse<NumberSequenceResponse>(validation));
        }

        var response = await settingsService.UpdateNumberSequenceAsync(documentType, request, cancellationToken);
        return ToActionResult(response);
    }

    private ActionResult<ApiResponse<TResponse>> ToActionResult<TResponse>(ApiResponse<TResponse> response)
    {
        return response.Success ? Ok(response) : BadRequest(response);
    }

    private static ApiResponse<TResponse> ToValidationResponse<TResponse>(ValidationResult validation)
    {
        var errors = validation.Errors.Select(error => error.ErrorMessage).ToArray();
        return ApiResponse.Fail<TResponse>("Gecersiz istek.", errors);
    }
}
