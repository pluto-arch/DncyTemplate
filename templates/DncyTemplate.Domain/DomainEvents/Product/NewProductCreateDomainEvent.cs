using MediatR;

namespace DncyTemplate.Domain.DomainEvents.Product;

/// <summary>
/// 新产品创建领域事件
/// </summary>
public class NewProductCreateDomainEvent : INotification
{
    public NewProductCreateDomainEvent(Aggregates.Product.Product prodId)
    {
        Id = prodId;
    }
    public Aggregates.Product.Product Id { get; set; }
}