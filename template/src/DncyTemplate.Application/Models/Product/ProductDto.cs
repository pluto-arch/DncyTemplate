namespace DncyTemplate.Application.Models.Product;

public class ProductDto : ProductListItemDto
{
    public ProductDto()
    {
        CreateTime = DateTime.Now;
    }

    public ProductDto(string id, string name, DateTime createTime)
    {
        Id = id;
        Name = name;
        CreateTime = createTime;
    }

    /// <summary>
    /// 描述信息
    /// </summary>
    public string Remark { get; set; }
}