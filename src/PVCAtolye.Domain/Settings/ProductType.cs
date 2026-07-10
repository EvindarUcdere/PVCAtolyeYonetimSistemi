namespace PVCAtolye.Domain.Settings;

public sealed class ProductType : DefinitionEntity
{
    private ProductType() { }
    public ProductType(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
