using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Customers;

public sealed class CustomerAddress : BaseEntity
{
    private CustomerAddress()
    {
    }

    public CustomerAddress(string title, string addressLine, string? district, string? city, string? postalCode, bool isDefault)
    {
        Title = title.Trim();
        AddressLine = addressLine.Trim();
        District = district?.Trim();
        City = city?.Trim();
        PostalCode = postalCode?.Trim();
        IsDefault = isDefault;
    }

    public Guid CustomerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string AddressLine { get; private set; } = string.Empty;
    public string? District { get; private set; }
    public string? City { get; private set; }
    public string? PostalCode { get; private set; }
    public bool IsDefault { get; private set; }
}
