namespace PVCAtolye.Application.Settings;

public sealed record DefinitionItemRequest(string Code, string Name, string? Description, int SortOrder, bool IsActive);
