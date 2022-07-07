namespace DncyTemplate.Application.Models.Product;

public class ProductItemModel
{
    public ProductItemModel()
    {
        CreateTime=DateTime.Now;
    }

    public ProductItemModel(string id, string name, DateTime createTime)
    {
        Id = id;
        Name = name;
        CreateTime = createTime;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public DateTime CreateTime { get; set; }
}