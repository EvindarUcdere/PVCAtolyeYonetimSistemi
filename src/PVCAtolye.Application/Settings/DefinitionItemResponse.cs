namespace PVCAtolye.Application.Settings;

public sealed record DefinitionItemResponse(Guid Id, string Code, string Name, string? Description, int SortOrder, bool IsActive);
