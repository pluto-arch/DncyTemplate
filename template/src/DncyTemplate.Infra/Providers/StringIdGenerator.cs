using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DncyTemplate.Infra.Providers;

public class StringIdGenerator : ValueGenerator<string>
{
    private readonly object _lock = new();

    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry hosts)
    {
        lock (_lock)
        {
            long stamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            string id = $"{stamp}{Random.Shared.Next(10000, 100000)}";
            return id;
        }
    }
}