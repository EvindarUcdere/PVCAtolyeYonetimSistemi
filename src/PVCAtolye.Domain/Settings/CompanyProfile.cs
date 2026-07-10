using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Settings;

public sealed class CompanyProfile : AuditableEntity
{
    private CompanyProfile()
    {
    }

    public CompanyProfile(string companyName)
    {
        CompanyName = companyName.Trim();
        CurrencyCode = "TRY";
        DefaultVatRate = 20m;
    }

    public string CompanyName { get; private set; } = string.Empty;
    public string? TaxOffice { get; private set; }
    public string? TaxNumber { get; private set; }
    public string? Address { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string CurrencyCode { get; private set; } = "TRY";
    public decimal DefaultVatRate { get; private set; }
    public string? QuoteFooterNote { get; private set; }

    public void Update(string companyName, string? taxOffice, string? taxNumber, string? address, string? phone, string? email, string currencyCode, decimal defaultVatRate, string? quoteFooterNote)
    {
        CompanyName = companyName.Trim();
        TaxOffice = taxOffice?.Trim();
        TaxNumber = taxNumber?.Trim();
        Address = address?.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
        DefaultVatRate = defaultVatRate;
        QuoteFooterNote = quoteFooterNote?.Trim();
    }
}
