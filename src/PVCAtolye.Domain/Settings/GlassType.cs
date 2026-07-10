namespace PVCAtolye.Domain.Settings;

public sealed class GlassType : DefinitionEntity
{
    private GlassType() { }
    public GlassType(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
