namespace PVCAtolye.Domain.Settings;

public sealed class ProductionStage : DefinitionEntity
{
    private ProductionStage() { }
    public ProductionStage(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
