using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Customers;

public sealed class CustomerContact : BaseEntity
{
    private CustomerContact()
    {
    }

    public CustomerContact(string fullName, string? title, string? phone, string? email, bool isPrimary)
    {
        FullName = fullName.Trim();
        Title = title?.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim();
        IsPrimary = isPrimary;
    }

    public Guid CustomerId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string? Title { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public bool IsPrimary { get; private set; }
}
