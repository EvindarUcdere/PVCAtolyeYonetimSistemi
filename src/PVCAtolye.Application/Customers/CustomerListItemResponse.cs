using PVCAtolye.Domain.Customers;

namespace PVCAtolye.Application.Customers;

public sealed record CustomerListItemResponse(Guid Id, CustomerType Type, string DisplayName, string? Phone, string? Email, string? TaxNumber, bool IsActive);
