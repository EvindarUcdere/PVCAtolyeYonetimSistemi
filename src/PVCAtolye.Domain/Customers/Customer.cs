using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Customers;

public sealed class Customer : AuditableEntity
{
    private readonly List<CustomerAddress> addresses = [];
    private readonly List<CustomerContact> contacts = [];

    private Customer()
    {
    }

    public Customer(CustomerType type, string displayName, string? taxOffice, string? taxNumber, string? identityNumber, string? phone, string? email, string? notes)
    {
        Type = type;
        DisplayName = displayName.Trim();
        TaxOffice = taxOffice?.Trim();
        TaxNumber = taxNumber?.Trim();
        IdentityNumber = identityNumber?.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        Notes = notes?.Trim();
        IsActive = true;
    }

    public CustomerType Type { get; private set; }
    public string DisplayName { get; private set; } = string.Empty;
    public string? TaxOffice { get; private set; }
    public string? TaxNumber { get; private set; }
    public string? IdentityNumber { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<CustomerAddress> Addresses => addresses;
    public IReadOnlyCollection<CustomerContact> Contacts => contacts;

    public void Update(CustomerType type, string displayName, string? taxOffice, string? taxNumber, string? identityNumber, string? phone, string? email, string? notes, bool isActive)
    {
        Type = type;
        DisplayName = displayName.Trim();
        TaxOffice = taxOffice?.Trim();
        TaxNumber = taxNumber?.Trim();
        IdentityNumber = identityNumber?.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        Notes = notes?.Trim();
        IsActive = isActive;
    }

    public void ReplaceAddresses(IEnumerable<CustomerAddress> newAddresses)
    {
        addresses.Clear();
        addresses.AddRange(newAddresses);
    }

    public void ReplaceContacts(IEnumerable<CustomerContact> newContacts)
    {
        contacts.Clear();
        contacts.AddRange(newContacts);
    }
}
