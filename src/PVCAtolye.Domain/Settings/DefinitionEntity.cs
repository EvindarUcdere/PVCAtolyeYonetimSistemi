using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Settings;

public abstract class DefinitionEntity : AuditableEntity
{
    protected DefinitionEntity()
    {
    }

    protected DefinitionEntity(string code, string name, string? description, int sortOrder)
    {
        Code = code.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        SortOrder = sortOrder;
        IsActive = true;
    }

    public string Code { get; protected set; } = string.Empty;
    public string Name { get; protected set; } = string.Empty;
    public string? Description { get; protected set; }
    public int SortOrder { get; protected set; }
    public bool IsActive { get; protected set; }

    public void Update(string code, string name, string? description, int sortOrder, bool isActive)
    {
        Code = code.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        SortOrder = sortOrder;
        IsActive = isActive;
    }
}
