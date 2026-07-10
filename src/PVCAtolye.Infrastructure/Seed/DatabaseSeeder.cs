using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PVCAtolye.Application.Common.Security;
using PVCAtolye.Domain.Audit;
using PVCAtolye.Domain.Identity;
using PVCAtolye.Domain.Settings;
using PVCAtolye.Infrastructure.Identity;
using PVCAtolye.Infrastructure.Persistence;

namespace PVCAtolye.Infrastructure.Seed;

public sealed class DatabaseSeeder(AppDbContext dbContext, IPasswordHasher passwordHasher, IOptions<AdminSeedOptions> options)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedPermissionsAsync(cancellationToken);
        var ownerRole = await SeedOwnerRoleAsync(cancellationToken);
        await SeedAdminUserAsync(ownerRole, cancellationToken);
        await SeedSettingsAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPermissionsAsync(CancellationToken cancellationToken)
    {
        foreach (var code in PermissionCodes.All)
        {
            if (!await dbContext.Permissions.AnyAsync(permission => permission.Code == code, cancellationToken))
            {
                dbContext.Permissions.Add(new Permission(code, code));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Role> SeedOwnerRoleAsync(CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .Include(entity => entity.RolePermissions)
            .SingleOrDefaultAsync(entity => entity.NormalizedName == "ISLETME SAHIBI", cancellationToken);

        if (role is null)
        {
            role = new Role("Isletme Sahibi", "Tum sistem yetkilerine sahip rol.");
            dbContext.Roles.Add(role);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var permissions = await dbContext.Permissions.ToListAsync(cancellationToken);
        foreach (var permission in permissions)
        {
            role.AddPermission(permission);
        }

        return role;
    }

    private async Task SeedAdminUserAsync(Role ownerRole, CancellationToken cancellationToken)
    {
        var adminOptions = options.Value;
        var normalizedUsername = adminOptions.Username.Trim().ToUpperInvariant();
        var admin = await dbContext.Users
            .Include(entity => entity.UserRoles)
            .SingleOrDefaultAsync(entity => entity.NormalizedUsername == normalizedUsername, cancellationToken);

        if (admin is not null)
        {
            return;
        }

        admin = new User(adminOptions.Username, adminOptions.DisplayName, passwordHasher.HashPassword(adminOptions.Password), mustChangePassword: true);
        admin.AddRole(ownerRole);
        dbContext.Users.Add(admin);
        dbContext.AuditLogs.Add(new AuditLog(null, "Seed.AdminCreated", nameof(User), admin.Id.ToString(), DateTimeOffset.UtcNow, "Varsayilan admin kullanicisi olusturuldu."));
    }

    private async Task SeedSettingsAsync(CancellationToken cancellationToken)
    {
        if (!await dbContext.CompanyProfiles.AnyAsync(cancellationToken))
        {
            dbContext.CompanyProfiles.Add(new CompanyProfile("PVC Atolye"));
        }

        if (!await dbContext.Warehouses.AnyAsync(entity => entity.Code == "MAIN", cancellationToken))
        {
            dbContext.Warehouses.Add(new Warehouse("MAIN", "Ana Depo", null, isDefault: true));
        }

        await SeedDefinitionsAsync(dbContext.UnitOfMeasures, static item => new UnitOfMeasure(item.Code, item.Name, item.Description, item.SortOrder), [
            new("M", "Metre", null, 10),
            new("BOY", "Boy", null, 20),
            new("ADET", "Adet", null, 30),
            new("M2", "Metrekare", null, 40),
            new("KG", "Kilogram", null, 50),
            new("PAKET", "Paket", null, 60),
            new("TAKIM", "Takim", null, 70),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.MaterialCategories, static item => new MaterialCategory(item.Code, item.Name, item.Description, item.SortOrder), [
            new("PVC_PROFIL", "PVC Profil", null, 10),
            new("DESTEK_SACI", "Destek Saci", null, 20),
            new("CAM", "Cam", null, 30),
            new("CONTA", "Conta", null, 40),
            new("KOL", "Kol", null, 50),
            new("MENTESE", "Mentese", null, 60),
            new("ISPANYOLET", "Ispanyolet", null, 70),
            new("KILIT", "Kilit", null, 80),
            new("VIDA", "Vida", null, 90),
            new("SINEKLIK", "Sineklik", null, 100),
            new("PANJUR", "Panjur", null, 110),
            new("DENIZLIK", "Denizlik", null, 120),
            new("MERMER", "Mermer", null, 130),
            new("YARDIMCI", "Yardimci Malzeme", null, 140),
            new("AKSESUAR", "Aksesuar", null, 150),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.ProductTypes, static item => new ProductType(item.Code, item.Name, item.Description, item.SortOrder), [
            new("SABIT_PENCERE", "Sabit Pencere", null, 10),
            new("TEK_ACILIM", "Tek Acilim", null, 20),
            new("CIFT_ACILIM", "Cift Acilim", null, 30),
            new("CIFT_KANAT", "Cift Kanat", null, 40),
            new("BALKON_KAPISI", "Balkon Kapisi", null, 50),
            new("SURME", "Surme", null, 60),
            new("KATLANIR", "Katlanir", null, 70),
            new("CAM_BALKON", "Cam Balkon", null, 80),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.ColorDefinitions, static item => new ColorDefinition(item.Code, item.Name, item.Description, item.SortOrder), [
            new("BEYAZ", "Beyaz", null, 10),
            new("ANTRASIT", "Antrasit", null, 20),
            new("MESE", "Mese", null, 30),
            new("CEVIZ", "Ceviz", null, 40),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.GlassTypes, static item => new GlassType(item.Code, item.Name, item.Description, item.SortOrder), [
            new("TEK_CAM", "Tek Cam", null, 10),
            new("CIFT_CAM", "Cift Cam", null, 20),
            new("LOW_E", "Low-E Cam", null, 30),
            new("TEMPERLI", "Temperli Cam", null, 40),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.ProfileSeries, static item => new ProfileSeries(item.Code, item.Name, item.Description, item.SortOrder), [
            new("STANDART_60", "Standart 60 Seri", null, 10),
            new("STANDART_70", "Standart 70 Seri", null, 20),
            new("SURME_SERI", "Surme Seri", null, 30),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.AccessoryTypes, static item => new AccessoryType(item.Code, item.Name, item.Description, item.SortOrder), [
            new("KOL", "Kol", null, 10),
            new("MENTESE", "Mentese", null, 20),
            new("KILIT", "Kilit", null, 30),
            new("ISPANYOLET", "Ispanyolet", null, 40),
            new("SINEKLIK", "Sineklik", null, 50),
            new("PANJUR", "Panjur", null, 60),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.ProductionStages, static item => new ProductionStage(item.Code, item.Name, item.Description, item.SortOrder), [
            new("MALZEME_HAZIRLIGI", "Malzeme Hazirligi", null, 10),
            new("PROFIL_KESIM", "Profil Kesim", null, 20),
            new("DESTEK_SACI", "Destek Saci", null, 30),
            new("KAYNAK", "Kaynak", null, 40),
            new("KOSE_TEMIZLEME", "Kose Temizleme", null, 50),
            new("AKSESUAR_MONTAJI", "Aksesuar Montaji", null, 60),
            new("CAM_MONTAJI", "Cam Montaji", null, 70),
            new("KALITE_KONTROL", "Kalite Kontrol", null, 80),
            new("PAKETLEME", "Paketleme", null, 90),
            new("MONTAJA_HAZIR", "Montaja Hazir", null, 100),
        ], cancellationToken);

        await SeedDefinitionsAsync(dbContext.PaymentMethods, static item => new PaymentMethod(item.Code, item.Name, item.Description, item.SortOrder), [
            new("NAKIT", "Nakit", null, 10),
            new("KREDI_KARTI", "Kredi Karti", null, 20),
            new("HAVALE", "Havale / EFT", null, 30),
            new("CEK", "Cek", null, 40),
            new("SENET", "Senet", null, 50),
            new("DIGER", "Diger", null, 60),
        ], cancellationToken);

        await SeedNumberSequenceAsync("QUOTE", "TKL-", 1, 6, cancellationToken);
        await SeedNumberSequenceAsync("ORDER", "SPR-", 1, 6, cancellationToken);
    }

    private static async Task SeedDefinitionsAsync<TDefinition>(
        DbSet<TDefinition> set,
        Func<DefinitionSeedItem, TDefinition> factory,
        IReadOnlyCollection<DefinitionSeedItem> items,
        CancellationToken cancellationToken)
        where TDefinition : DefinitionEntity
    {
        foreach (var item in items)
        {
            if (!await set.AnyAsync(entity => entity.Code == item.Code, cancellationToken))
            {
                set.Add(factory(item));
            }
        }
    }

    private async Task SeedNumberSequenceAsync(string documentType, string prefix, int nextNumber, int paddingLength, CancellationToken cancellationToken)
    {
        if (!await dbContext.NumberSequenceSettings.AnyAsync(entity => entity.DocumentType == documentType, cancellationToken))
        {
            dbContext.NumberSequenceSettings.Add(new NumberSequenceSetting(documentType, prefix, nextNumber, paddingLength));
        }
    }

    private sealed record DefinitionSeedItem(string Code, string Name, string? Description, int SortOrder);
}
