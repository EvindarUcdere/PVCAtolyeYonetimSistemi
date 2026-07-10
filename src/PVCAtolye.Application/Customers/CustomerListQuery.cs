namespace PVCAtolye.Application.Customers;

public sealed record CustomerListQuery(string? Search, int Page = 1, int PageSize = 20, bool IncludePassive = false);
