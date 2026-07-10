namespace PVCAtolye.Application.Settings;

public sealed record CompanyProfileResponse(Guid Id, string CompanyName, string? TaxOffice, string? TaxNumber, string? Address, string? Phone, string? Email, string CurrencyCode, decimal DefaultVatRate, string? QuoteFooterNote);
