using DncyTemplate.Domain.Aggregates.EventLogs;

namespace DncyTemplate.Application.IntegrationEvents.Events
{
    public class ProductCreatedIntegrationEvent : IntegrationEvent
    {
        public ProductCreatedIntegrationEvent(string productId) : base()
        {
            ProductId = productId;
        }

        public string ProductId { get; set; }
    }
}