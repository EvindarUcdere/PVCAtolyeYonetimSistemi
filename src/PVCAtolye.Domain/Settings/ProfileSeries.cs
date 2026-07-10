namespace PVCAtolye.Domain.Settings;

public sealed class ProfileSeries : DefinitionEntity
{
    private ProfileSeries() { }
    public ProfileSeries(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
