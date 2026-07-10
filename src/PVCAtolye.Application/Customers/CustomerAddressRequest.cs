namespace PVCAtolye.Application.Customers;

public sealed record CustomerAddressRequest(string Title, string AddressLine, string? District, string? City, string? PostalCode, bool IsDefault);
