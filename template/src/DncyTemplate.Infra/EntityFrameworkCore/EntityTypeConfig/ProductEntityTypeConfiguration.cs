using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Aggregates.System;
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
        ValueConverter<GeoCoordinate, string> converter = new(v => v, v => (GeoCoordinate)v);
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
        builder.ToTable($"Products");
        builder.HasKey(e => e.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<StringIdGenerator>();
        builder.Ignore(e => e.DomainEvents);
        builder.Property(e => e.Name).HasMaxLength(20);
        builder.Property(e => e.Remark).HasMaxLength(100);
        builder.Property(e => e.CreationTime).IsRequired().HasDefaultValueSql("GETDATE()");
    }
}


public class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<PermissionGrant>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PermissionGrant> builder)
    {
        builder.ToTable("PermissionGrant");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
        builder.Property(x => x.ProviderName).HasMaxLength(300).IsRequired();
        builder.Property(x => x.ProviderKey).HasMaxLength(300).IsRequired();
    }
}
