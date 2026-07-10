using Microsoft.EntityFrameworkCore;
using PVCAtolye.Domain.Audit;
using PVCAtolye.Domain.Customers;
using PVCAtolye.Domain.Identity;
using PVCAtolye.Domain.Settings;

namespace PVCAtolye.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<CustomerContact> CustomerContacts => Set<CustomerContact>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<UnitOfMeasure> UnitOfMeasures => Set<UnitOfMeasure>();
    public DbSet<MaterialCategory> MaterialCategories => Set<MaterialCategory>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<ColorDefinition> ColorDefinitions => Set<ColorDefinition>();
    public DbSet<GlassType> GlassTypes => Set<GlassType>();
    public DbSet<ProfileSeries> ProfileSeries => Set<ProfileSeries>();
    public DbSet<AccessoryType> AccessoryTypes => Set<AccessoryType>();
    public DbSet<ProductionStage> ProductionStages => Set<ProductionStage>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<NumberSequenceSetting> NumberSequenceSettings => Set<NumberSequenceSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}
