namespace PVCAtolye.Infrastructure.Seed;

public sealed class AdminSeedOptions
{
    public const string SectionName = "AdminSeed";
    public string Username { get; set; } = "admin";
    public string DisplayName { get; set; } = "Sistem Yoneticisi";
    public string Password { get; set; } = "Admin123!";
}
