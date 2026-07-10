using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCAtolye.Domain.Customers;

namespace PVCAtolye.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(entity => entity.Id);
        builder.HasIndex(entity => entity.DisplayName);
        builder.HasIndex(entity => entity.Phone);
        builder.HasIndex(entity => entity.Email);
        builder.HasIndex(entity => entity.TaxNumber);
        builder.HasIndex(entity => entity.IdentityNumber);
        builder.Property(entity => entity.Type).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(entity => entity.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.TaxOffice).HasMaxLength(100);
        builder.Property(entity => entity.TaxNumber).HasMaxLength(50);
        builder.Property(entity => entity.IdentityNumber).HasMaxLength(20);
        builder.Property(entity => entity.Phone).HasMaxLength(50);
        builder.Property(entity => entity.Email).HasMaxLength(150);
        builder.Property(entity => entity.Notes).HasMaxLength(1000);

        builder.HasMany(entity => entity.Addresses)
            .WithOne()
            .HasForeignKey(entity => entity.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(entity => entity.Contacts)
            .WithOne()
            .HasForeignKey(entity => entity.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("customer_addresses");
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Title).HasMaxLength(80).IsRequired();
        builder.Property(entity => entity.AddressLine).HasMaxLength(500).IsRequired();
        builder.Property(entity => entity.District).HasMaxLength(100);
        builder.Property(entity => entity.City).HasMaxLength(100);
        builder.Property(entity => entity.PostalCode).HasMaxLength(20);
    }
}

public sealed class CustomerContactConfiguration : IEntityTypeConfiguration<CustomerContact>
{
    public void Configure(EntityTypeBuilder<CustomerContact> builder)
    {
        builder.ToTable("customer_contacts");
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.FullName).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.Title).HasMaxLength(100);
        builder.Property(entity => entity.Phone).HasMaxLength(50);
        builder.Property(entity => entity.Email).HasMaxLength(150);
    }
}
