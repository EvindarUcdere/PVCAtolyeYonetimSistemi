using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Settings;

public sealed class Warehouse : AuditableEntity
{
    private Warehouse()
    {
    }

    public Warehouse(string code, string name, string? address, bool isDefault)
    {
        Code = code.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        IsDefault = isDefault;
        IsActive = true;
    }

    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }

    public void Update(string code, string name, string? address, bool isDefault, bool isActive)
    {
        Code = code.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        IsDefault = isDefault;
        IsActive = isActive;
    }
}
