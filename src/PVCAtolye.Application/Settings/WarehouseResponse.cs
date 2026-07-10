namespace PVCAtolye.Application.Settings;

public sealed record WarehouseResponse(Guid Id, string Code, string Name, string? Address, bool IsDefault, bool IsActive);
