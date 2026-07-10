using PVCAtolye.Application.Common.Models;

namespace PVCAtolye.Application.Customers;

public interface ICustomerService
{
    Task<ApiResponse<PagedResponse<CustomerListItemResponse>>> GetCustomersAsync(CustomerListQuery query, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerDetailResponse>> GetCustomerAsync(Guid id, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerDetailResponse>> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<CustomerDetailResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request, CancellationToken cancellationToken);
}
