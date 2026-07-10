namespace PVCAtolye.Domain.Settings;

public sealed class MaterialCategory : DefinitionEntity
{
    private MaterialCategory() { }
    public MaterialCategory(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
