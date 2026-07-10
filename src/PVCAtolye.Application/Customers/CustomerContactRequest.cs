namespace PVCAtolye.Application.Customers;

public sealed record CustomerContactRequest(string FullName, string? Title, string? Phone, string? Email, bool IsPrimary);
