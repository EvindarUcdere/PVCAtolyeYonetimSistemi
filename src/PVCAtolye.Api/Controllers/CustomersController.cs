using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PVCAtolye.Application.Common.Models;
using PVCAtolye.Application.Common.Security;
using PVCAtolye.Application.Customers;

namespace PVCAtolye.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize(Policy = PermissionCodes.CustomersRead)]
public sealed class CustomersController(ICustomerService customerService, IValidator<CustomerRequest> validator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<CustomerListItemResponse>>>> GetCustomers(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool includePassive = false,
        CancellationToken cancellationToken = default)
    {
        var response = await customerService.GetCustomersAsync(new CustomerListQuery(search, page, pageSize, includePassive), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDetailResponse>>> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        var response = await customerService.GetCustomerAsync(id, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.CustomersManage)]
    public async Task<ActionResult<ApiResponse<CustomerDetailResponse>>> CreateCustomer(CustomerRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse(validation));
        }

        var response = await customerService.CreateCustomerAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCodes.CustomersManage)]
    public async Task<ActionResult<ApiResponse<CustomerDetailResponse>>> UpdateCustomer(Guid id, CustomerRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return BadRequest(ToValidationResponse(validation));
        }

        var response = await customerService.UpdateCustomerAsync(id, request, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    private static ApiResponse<CustomerDetailResponse> ToValidationResponse(ValidationResult validation)
    {
        var errors = validation.Errors.Select(error => error.ErrorMessage).ToArray();
        return ApiResponse.Fail<CustomerDetailResponse>("Gecersiz istek.", errors);
    }
}
