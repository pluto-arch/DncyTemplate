
namespace DncyTemplate.Domain.DomainEvents.Product;

/// <summary>
/// 新产品创建领域事件
/// </summary>
public class NewProductCreateDomainEvent : INotification
{
    public NewProductCreateDomainEvent(Aggregates.Product.Product prod)
    {
        Prod = prod;
    }
    public Aggregates.Product.Product Prod { get; set; }
}