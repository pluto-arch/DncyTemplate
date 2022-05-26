using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Domain.Aggregates.Product;

public class DeviceAddress: ValueObject
{
    public DeviceAddress() { }

    public DeviceAddress(string street, string city, string state, string country, string zipcode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipcode;
    }

    public string Street { get; }

    public string City { get; }

    public string State { get; }

    public string Country { get; }

    public string ZipCode { get; }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}