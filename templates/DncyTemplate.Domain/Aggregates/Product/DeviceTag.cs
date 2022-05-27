using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Domain.Aggregates.Product;

public class DeviceTag : BaseEntity<int>
{
    public string Name { get; set; }
}
