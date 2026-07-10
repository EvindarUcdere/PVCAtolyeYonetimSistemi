using PVCAtolye.Domain.Customers;

namespace PVCAtolye.Application.Customers;

public sealed record CustomerRequest(
    CustomerType Type,
    string DisplayName,
    string? TaxOffice,
    string? TaxNumber,
    string? IdentityNumber,
    string? Phone,
    string? Email,
    string? Notes,
    bool IsActive,
    IReadOnlyCollection<CustomerAddressRequest> Addresses,
    IReadOnlyCollection<CustomerContactRequest> Contacts);
