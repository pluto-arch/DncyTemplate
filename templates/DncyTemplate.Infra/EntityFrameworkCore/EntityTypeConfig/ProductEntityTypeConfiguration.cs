using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Infra.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DncyTemplate.Infra.EntityFrameworkCore.EntityTypeConfig;


public class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(20);
        builder.Property(e => e.SerialNo).IsRequired().HasMaxLength(36);
        ValueConverter<GeoCoordinate, string> converter = new (v => v, v => (GeoCoordinate)v);
        builder.Property(e => e.Coordinate).HasConversion(converter);
        builder.OwnsOne(e => e.Address, da => da.ToTable("DeviceAddresses"));
    }
}


public class DeviceTagEntityTypeConfiguration : IEntityTypeConfiguration<DeviceTag>
{
    public void Configure(EntityTypeBuilder<DeviceTag> builder)
    {
        builder.ToTable("DeviceTags");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(20);
    }
}


public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<StringIdGenerator>();
        builder.Ignore(e => e.DomainEvents);
        builder.Property(e => e.Name).HasMaxLength(20);
        builder.Property(e => e.Remark).HasMaxLength(100);
    }
}
