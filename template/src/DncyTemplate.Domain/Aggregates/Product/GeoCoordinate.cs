namespace DncyTemplate.Domain.Aggregates.Product;

public class GeoCoordinate
{
    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public override string ToString()
    {
        return $"{Latitude},{Longitude}";
    }


    /// <summary>
    ///     解构数据
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    public void Deconstruct(out double? latitude, out double? longitude)
    {
        latitude = Latitude;
        longitude = Longitude;
    }

    /// <summary>
    ///     隐式数据转换
    /// </summary>
    /// <param name="geo"></param>
    public static implicit operator string(GeoCoordinate geo)
    {
        return geo.ToString();
    }

    /// <summary>
    ///     显示数据转换
    /// </summary>
    /// <param name="str"></param>
    public static explicit operator GeoCoordinate(string str)
    {
        GeoCoordinate geoCoordinate = new();

        if (!string.IsNullOrWhiteSpace(str))
        {
            double[] arr = Array.ConvertAll(str.Split(','), Convert.ToDouble);
            geoCoordinate.Latitude = arr.First();
            geoCoordinate.Longitude = arr.Last();
        }

        return geoCoordinate;
    }
}