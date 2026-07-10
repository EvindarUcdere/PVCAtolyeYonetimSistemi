namespace PVCAtolye.Application.Customers;

public sealed record CustomerAddressResponse(Guid Id, string Title, string AddressLine, string? District, string? City, string? PostalCode, bool IsDefault);
