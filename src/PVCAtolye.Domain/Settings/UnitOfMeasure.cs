namespace PVCAtolye.Domain.Settings;

public sealed class UnitOfMeasure : DefinitionEntity
{
    private UnitOfMeasure() { }
    public UnitOfMeasure(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
