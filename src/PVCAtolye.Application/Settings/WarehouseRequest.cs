namespace PVCAtolye.Application.Settings;

public sealed record WarehouseRequest(string Code, string Name, string? Address, bool IsDefault, bool IsActive);
