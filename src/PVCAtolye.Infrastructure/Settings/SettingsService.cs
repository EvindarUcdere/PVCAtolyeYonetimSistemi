using Microsoft.EntityFrameworkCore;
using PVCAtolye.Application.Common.Models;
using PVCAtolye.Application.Settings;
using PVCAtolye.Domain.Settings;
using PVCAtolye.Infrastructure.Persistence;

namespace PVCAtolye.Infrastructure.Settings;

public sealed class SettingsService(AppDbContext dbContext) : ISettingsService
{
    public async Task<ApiResponse<CompanyProfileResponse>> GetCompanyProfileAsync(CancellationToken cancellationToken)
    {
        var profile = await dbContext.CompanyProfiles.AsNoTracking().OrderBy(entity => entity.CreatedAt).FirstOrDefaultAsync(cancellationToken);
        return profile is null
            ? ApiResponse.Fail<CompanyProfileResponse>("Isletme profili bulunamadi.")
            : ApiResponse.Ok(Map(profile));
    }

    public async Task<ApiResponse<CompanyProfileResponse>> UpdateCompanyProfileAsync(CompanyProfileRequest request, CancellationToken cancellationToken)
    {
        var profile = await dbContext.CompanyProfiles.OrderBy(entity => entity.CreatedAt).FirstOrDefaultAsync(cancellationToken);
        if (profile is null)
        {
            profile = new CompanyProfile(request.CompanyName);
            dbContext.CompanyProfiles.Add(profile);
        }

        profile.Update(request.CompanyName, request.TaxOffice, request.TaxNumber, request.Address, request.Phone, request.Email, request.CurrencyCode, request.DefaultVatRate, request.QuoteFooterNote);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(profile), "Isletme profili guncellendi.");
    }

    public async Task<ApiResponse<IReadOnlyCollection<WarehouseResponse>>> GetWarehousesAsync(CancellationToken cancellationToken)
    {
        var warehouses = await dbContext.Warehouses
            .AsNoTracking()
            .OrderByDescending(entity => entity.IsDefault)
            .ThenBy(entity => entity.Code)
            .Select(entity => Map(entity))
            .ToListAsync(cancellationToken);

        return ApiResponse.Ok<IReadOnlyCollection<WarehouseResponse>>(warehouses);
    }

    public async Task<ApiResponse<WarehouseResponse>> CreateWarehouseAsync(WarehouseRequest request, CancellationToken cancellationToken)
    {
        var code = NormalizeCode(request.Code);
        if (await dbContext.Warehouses.AnyAsync(entity => entity.Code == code, cancellationToken))
        {
            return ApiResponse.Fail<WarehouseResponse>("Depo kodu zaten kullaniliyor.");
        }

        if (request.IsDefault)
        {
            await ClearDefaultWarehousesAsync(cancellationToken);
        }

        var warehouse = new Warehouse(code, request.Name, request.Address, request.IsDefault);
        dbContext.Warehouses.Add(warehouse);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(warehouse), "Depo olusturuldu.");
    }

    public async Task<ApiResponse<WarehouseResponse>> UpdateWarehouseAsync(Guid id, WarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouse = await dbContext.Warehouses.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        if (warehouse is null)
        {
            return ApiResponse.Fail<WarehouseResponse>("Depo bulunamadi.");
        }

        var code = NormalizeCode(request.Code);
        if (await dbContext.Warehouses.AnyAsync(entity => entity.Id != id && entity.Code == code, cancellationToken))
        {
            return ApiResponse.Fail<WarehouseResponse>("Depo kodu zaten kullaniliyor.");
        }

        if (request.IsDefault)
        {
            await ClearDefaultWarehousesAsync(cancellationToken);
        }

        warehouse.Update(code, request.Name, request.Address, request.IsDefault, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(warehouse), "Depo guncellendi.");
    }

    public Task<ApiResponse<IReadOnlyCollection<DefinitionItemResponse>>> GetDefinitionsAsync(string definitionType, CancellationToken cancellationToken)
    {
        return NormalizeDefinitionType(definitionType) switch
        {
            "units" => GetDefinitionsCoreAsync(dbContext.UnitOfMeasures, cancellationToken),
            "material-categories" => GetDefinitionsCoreAsync(dbContext.MaterialCategories, cancellationToken),
            "product-types" => GetDefinitionsCoreAsync(dbContext.ProductTypes, cancellationToken),
            "colors" => GetDefinitionsCoreAsync(dbContext.ColorDefinitions, cancellationToken),
            "glass-types" => GetDefinitionsCoreAsync(dbContext.GlassTypes, cancellationToken),
            "profile-series" => GetDefinitionsCoreAsync(dbContext.ProfileSeries, cancellationToken),
            "accessory-types" => GetDefinitionsCoreAsync(dbContext.AccessoryTypes, cancellationToken),
            "production-stages" => GetDefinitionsCoreAsync(dbContext.ProductionStages, cancellationToken),
            "payment-methods" => GetDefinitionsCoreAsync(dbContext.PaymentMethods, cancellationToken),
            _ => Task.FromResult(ApiResponse.Fail<IReadOnlyCollection<DefinitionItemResponse>>("Gecersiz tanim tipi."))
        };
    }

    public Task<ApiResponse<DefinitionItemResponse>> CreateDefinitionAsync(string definitionType, DefinitionItemRequest request, CancellationToken cancellationToken)
    {
        return NormalizeDefinitionType(definitionType) switch
        {
            "units" => CreateDefinitionCoreAsync(dbContext.UnitOfMeasures, request, static (code, name, description, sortOrder) => new UnitOfMeasure(code, name, description, sortOrder), cancellationToken),
            "material-categories" => CreateDefinitionCoreAsync(dbContext.MaterialCategories, request, static (code, name, description, sortOrder) => new MaterialCategory(code, name, description, sortOrder), cancellationToken),
            "product-types" => CreateDefinitionCoreAsync(dbContext.ProductTypes, request, static (code, name, description, sortOrder) => new ProductType(code, name, description, sortOrder), cancellationToken),
            "colors" => CreateDefinitionCoreAsync(dbContext.ColorDefinitions, request, static (code, name, description, sortOrder) => new ColorDefinition(code, name, description, sortOrder), cancellationToken),
            "glass-types" => CreateDefinitionCoreAsync(dbContext.GlassTypes, request, static (code, name, description, sortOrder) => new GlassType(code, name, description, sortOrder), cancellationToken),
            "profile-series" => CreateDefinitionCoreAsync(dbContext.ProfileSeries, request, static (code, name, description, sortOrder) => new ProfileSeries(code, name, description, sortOrder), cancellationToken),
            "accessory-types" => CreateDefinitionCoreAsync(dbContext.AccessoryTypes, request, static (code, name, description, sortOrder) => new AccessoryType(code, name, description, sortOrder), cancellationToken),
            "production-stages" => CreateDefinitionCoreAsync(dbContext.ProductionStages, request, static (code, name, description, sortOrder) => new ProductionStage(code, name, description, sortOrder), cancellationToken),
            "payment-methods" => CreateDefinitionCoreAsync(dbContext.PaymentMethods, request, static (code, name, description, sortOrder) => new PaymentMethod(code, name, description, sortOrder), cancellationToken),
            _ => Task.FromResult(ApiResponse.Fail<DefinitionItemResponse>("Gecersiz tanim tipi."))
        };
    }

    public Task<ApiResponse<DefinitionItemResponse>> UpdateDefinitionAsync(string definitionType, Guid id, DefinitionItemRequest request, CancellationToken cancellationToken)
    {
        return NormalizeDefinitionType(definitionType) switch
        {
            "units" => UpdateDefinitionCoreAsync(dbContext.UnitOfMeasures, id, request, cancellationToken),
            "material-categories" => UpdateDefinitionCoreAsync(dbContext.MaterialCategories, id, request, cancellationToken),
            "product-types" => UpdateDefinitionCoreAsync(dbContext.ProductTypes, id, request, cancellationToken),
            "colors" => UpdateDefinitionCoreAsync(dbContext.ColorDefinitions, id, request, cancellationToken),
            "glass-types" => UpdateDefinitionCoreAsync(dbContext.GlassTypes, id, request, cancellationToken),
            "profile-series" => UpdateDefinitionCoreAsync(dbContext.ProfileSeries, id, request, cancellationToken),
            "accessory-types" => UpdateDefinitionCoreAsync(dbContext.AccessoryTypes, id, request, cancellationToken),
            "production-stages" => UpdateDefinitionCoreAsync(dbContext.ProductionStages, id, request, cancellationToken),
            "payment-methods" => UpdateDefinitionCoreAsync(dbContext.PaymentMethods, id, request, cancellationToken),
            _ => Task.FromResult(ApiResponse.Fail<DefinitionItemResponse>("Gecersiz tanim tipi."))
        };
    }

    public async Task<ApiResponse<IReadOnlyCollection<NumberSequenceResponse>>> GetNumberSequencesAsync(CancellationToken cancellationToken)
    {
        var sequences = await dbContext.NumberSequenceSettings
            .AsNoTracking()
            .OrderBy(entity => entity.DocumentType)
            .Select(entity => Map(entity))
            .ToListAsync(cancellationToken);

        return ApiResponse.Ok<IReadOnlyCollection<NumberSequenceResponse>>(sequences);
    }

    public async Task<ApiResponse<NumberSequenceResponse>> UpdateNumberSequenceAsync(string documentType, NumberSequenceRequest request, CancellationToken cancellationToken)
    {
        var normalizedDocumentType = NormalizeCode(documentType);
        var sequence = await dbContext.NumberSequenceSettings.SingleOrDefaultAsync(entity => entity.DocumentType == normalizedDocumentType, cancellationToken);
        if (sequence is null)
        {
            return ApiResponse.Fail<NumberSequenceResponse>("Numara ayari bulunamadi.");
        }

        sequence.Update(request.Prefix, request.NextNumber, request.PaddingLength, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(sequence), "Numara ayari guncellendi.");
    }

    private static async Task<ApiResponse<IReadOnlyCollection<DefinitionItemResponse>>> GetDefinitionsCoreAsync<TDefinition>(
        DbSet<TDefinition> set,
        CancellationToken cancellationToken)
        where TDefinition : DefinitionEntity
    {
        var items = await set.AsNoTracking()
            .OrderBy(entity => entity.SortOrder)
            .ThenBy(entity => entity.Name)
            .Select(entity => Map(entity))
            .ToListAsync(cancellationToken);

        return ApiResponse.Ok<IReadOnlyCollection<DefinitionItemResponse>>(items);
    }

    private async Task<ApiResponse<DefinitionItemResponse>> CreateDefinitionCoreAsync<TDefinition>(
        DbSet<TDefinition> set,
        DefinitionItemRequest request,
        Func<string, string, string?, int, TDefinition> factory,
        CancellationToken cancellationToken)
        where TDefinition : DefinitionEntity
    {
        var code = NormalizeCode(request.Code);
        if (await set.AnyAsync(entity => entity.Code == code, cancellationToken))
        {
            return ApiResponse.Fail<DefinitionItemResponse>("Tanim kodu zaten kullaniliyor.");
        }

        var item = factory(code, request.Name, request.Description, request.SortOrder);
        set.Add(item);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(item), "Tanim olusturuldu.");
    }

    private async Task<ApiResponse<DefinitionItemResponse>> UpdateDefinitionCoreAsync<TDefinition>(
        DbSet<TDefinition> set,
        Guid id,
        DefinitionItemRequest request,
        CancellationToken cancellationToken)
        where TDefinition : DefinitionEntity
    {
        var item = await set.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        if (item is null)
        {
            return ApiResponse.Fail<DefinitionItemResponse>("Tanim bulunamadi.");
        }

        var code = NormalizeCode(request.Code);
        if (await set.AnyAsync(entity => entity.Id != id && entity.Code == code, cancellationToken))
        {
            return ApiResponse.Fail<DefinitionItemResponse>("Tanim kodu zaten kullaniliyor.");
        }

        item.Update(code, request.Name, request.Description, request.SortOrder, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(item), "Tanim guncellendi.");
    }

    private static string NormalizeCode(string value) => value.Trim().ToUpperInvariant();

    private static string NormalizeDefinitionType(string value) => value.Trim().ToLowerInvariant();

    private async Task ClearDefaultWarehousesAsync(CancellationToken cancellationToken)
    {
        var defaults = await dbContext.Warehouses.Where(entity => entity.IsDefault).ToListAsync(cancellationToken);
        foreach (var warehouse in defaults)
        {
            warehouse.Update(warehouse.Code, warehouse.Name, warehouse.Address, isDefault: false, warehouse.IsActive);
        }
    }

    private static CompanyProfileResponse Map(CompanyProfile entity) =>
        new(entity.Id, entity.CompanyName, entity.TaxOffice, entity.TaxNumber, entity.Address, entity.Phone, entity.Email, entity.CurrencyCode, entity.DefaultVatRate, entity.QuoteFooterNote);

    private static WarehouseResponse Map(Warehouse entity) =>
        new(entity.Id, entity.Code, entity.Name, entity.Address, entity.IsDefault, entity.IsActive);

    private static DefinitionItemResponse Map(DefinitionEntity entity) =>
        new(entity.Id, entity.Code, entity.Name, entity.Description, entity.SortOrder, entity.IsActive);

    private static NumberSequenceResponse Map(NumberSequenceSetting entity) =>
        new(entity.Id, entity.DocumentType, entity.Prefix, entity.NextNumber, entity.PaddingLength, entity.IsActive);
}
