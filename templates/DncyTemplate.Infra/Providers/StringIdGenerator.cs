using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DncyTemplate.Infra.Providers;

public class StringIdGenerator: ValueGenerator<string>
{
    private readonly object _lock = new();

    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
    {
        lock (_lock)
        {
            long stamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            string id = $"{stamp}{RandomNumberString(5)}";
            return id;
        }
    }


    private static string RandomNumberString(int length)
    {
        string result = "";
        Random random = new(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < length; i++)
        {
            result += random.Next(10).ToString();
        }

        return result;
    }
}