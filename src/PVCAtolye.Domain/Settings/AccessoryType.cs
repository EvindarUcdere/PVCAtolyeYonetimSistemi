namespace PVCAtolye.Domain.Settings;

public sealed class AccessoryType : DefinitionEntity
{
    private AccessoryType() { }
    public AccessoryType(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
