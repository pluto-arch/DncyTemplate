namespace DncyTemplate.Application.Models.Product;

public class ProductListItemDto
{
    public ProductListItemDto()
    {
        CreateTime = DateTime.Now;
    }

    public ProductListItemDto(string id, string name, DateTime createTime)
    {
        Id = id;
        Name = name;
        CreateTime = createTime;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public DateTime CreateTime { get; set; }
}