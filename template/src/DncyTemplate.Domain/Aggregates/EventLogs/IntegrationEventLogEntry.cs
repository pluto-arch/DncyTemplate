using DncyTemplate.Domain.Infra;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DncyTemplate.Domain.Aggregates.EventLogs
{
     public class IntegrationEventLogEntry: BaseEntity
#if Tenant
        , IMultiTenant
#endif
    {

        private static readonly JsonSerializerOptions _indentedOptions = new() { WriteIndented = true };
        private static readonly JsonSerializerOptions _caseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };


        private IntegrationEventLogEntry() { }


        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonSerializer.Serialize(@event, @event.GetType(), _indentedOptions);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }

        public IntegrationEventLogEntry(IntegrationEvent @event, string transactionId)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonSerializer.Serialize(@event, @event.GetType(), _indentedOptions);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId;
        }

        [Key]
        public string EventId { get; private set; }

        [Required]
        public string EventTypeName { get; private set; }

        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();

        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }

        public EventStateEnum State { get; set; }

        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }

        [Required]
        public string Content { get; private set; }

        public string TransactionId { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonSerializer.Deserialize(Content, type, _caseInsensitiveOptions) as IntegrationEvent;
            return this;
        }

        public string TenantId { get; set; }


        public override object[] GetKeys()
        {
            return new object[] { EventId };
        }
    }
}