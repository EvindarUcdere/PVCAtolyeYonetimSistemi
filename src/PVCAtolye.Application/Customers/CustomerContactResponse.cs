namespace PVCAtolye.Application.Customers;

public sealed record CustomerContactResponse(Guid Id, string FullName, string? Title, string? Phone, string? Email, bool IsPrimary);
