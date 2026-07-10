using PVCAtolye.Domain.Customers;

namespace PVCAtolye.Application.Customers;

public sealed record CustomerDetailResponse(
    Guid Id,
    CustomerType Type,
    string DisplayName,
    string? TaxOffice,
    string? TaxNumber,
    string? IdentityNumber,
    string? Phone,
    string? Email,
    string? Notes,
    bool IsActive,
    IReadOnlyCollection<CustomerAddressResponse> Addresses,
    IReadOnlyCollection<CustomerContactResponse> Contacts);
