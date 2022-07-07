using DncyTemplate.Domain.Infra;

namespace DncyTemplate.Domain.Aggregates.Product;

public class Device : BaseEntity<int>, IMultiTenant
{
    /// <summary>
    ///     设备名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     序列号
    /// </summary>
    public string SerialNo { get; set; }


    /// <summary>
    ///     安装地址
    /// </summary>
    public DeviceAddress Address { get; set; }


    /// <summary>
    ///     经纬度
    /// </summary>
    public GeoCoordinate Coordinate { get; set; }


    /// <summary>
    ///     是否在线
    /// </summary>
    public bool Online { get; set; }


    /// <summary>
    ///     产品编号
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    ///     所属产品
    /// </summary>
    public Product Product { get; set; }


    public string TenantId { get; set; }
}