using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCAtolye.Domain.Settings;

namespace PVCAtolye.Infrastructure.Persistence.Configurations;

public sealed class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
{
    public void Configure(EntityTypeBuilder<CompanyProfile> builder)
    {
        builder.ToTable("company_profiles");
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.CompanyName).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.TaxOffice).HasMaxLength(100);
        builder.Property(entity => entity.TaxNumber).HasMaxLength(50);
        builder.Property(entity => entity.Address).HasMaxLength(500);
        builder.Property(entity => entity.Phone).HasMaxLength(50);
        builder.Property(entity => entity.Email).HasMaxLength(150);
        builder.Property(entity => entity.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(entity => entity.QuoteFooterNote).HasMaxLength(1000);
    }
}

public sealed class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("warehouses");
        builder.HasKey(entity => entity.Id);
        builder.HasIndex(entity => entity.Code).IsUnique();
        builder.Property(entity => entity.Code).HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.Name).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Address).HasMaxLength(500);
    }
}

public sealed class UnitOfMeasureConfiguration : DefinitionConfiguration<UnitOfMeasure>
{
    protected override string TableName => "unit_of_measures";
}

public sealed class MaterialCategoryConfiguration : DefinitionConfiguration<MaterialCategory>
{
    protected override string TableName => "material_categories";
}

public sealed class ProductTypeConfiguration : DefinitionConfiguration<ProductType>
{
    protected override string TableName => "product_types";
}

public sealed class ColorDefinitionConfiguration : DefinitionConfiguration<ColorDefinition>
{
    protected override string TableName => "color_definitions";
}

public sealed class GlassTypeConfiguration : DefinitionConfiguration<GlassType>
{
    protected override string TableName => "glass_types";
}

public sealed class ProfileSeriesConfiguration : DefinitionConfiguration<ProfileSeries>
{
    protected override string TableName => "profile_series";
}

public sealed class AccessoryTypeConfiguration : DefinitionConfiguration<AccessoryType>
{
    protected override string TableName => "accessory_types";
}

public sealed class ProductionStageConfiguration : DefinitionConfiguration<ProductionStage>
{
    protected override string TableName => "production_stages";
}

public sealed class PaymentMethodConfiguration : DefinitionConfiguration<PaymentMethod>
{
    protected override string TableName => "payment_methods";
}

public sealed class NumberSequenceSettingConfiguration : IEntityTypeConfiguration<NumberSequenceSetting>
{
    public void Configure(EntityTypeBuilder<NumberSequenceSetting> builder)
    {
        builder.ToTable("number_sequence_settings");
        builder.HasKey(entity => entity.Id);
        builder.HasIndex(entity => entity.DocumentType).IsUnique();
        builder.Property(entity => entity.DocumentType).HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.Prefix).HasMaxLength(20).IsRequired();
    }
}

public abstract class DefinitionConfiguration<TDefinition> : IEntityTypeConfiguration<TDefinition>
    where TDefinition : DefinitionEntity
{
    protected abstract string TableName { get; }

    public void Configure(EntityTypeBuilder<TDefinition> builder)
    {
        builder.ToTable(TableName);
        builder.HasKey(entity => entity.Id);
        builder.HasIndex(entity => entity.Code).IsUnique();
        builder.Property(entity => entity.Code).HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.Name).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Description).HasMaxLength(500);
    }
}
