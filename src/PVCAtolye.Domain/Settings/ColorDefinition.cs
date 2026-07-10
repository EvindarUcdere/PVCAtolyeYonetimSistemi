namespace PVCAtolye.Domain.Settings;

public sealed class ColorDefinition : DefinitionEntity
{
    private ColorDefinition() { }
    public ColorDefinition(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
