using System.Text.Json.Serialization;

namespace DncyTemplate.Domain.Aggregates.EventLogs
{
    /// <summary>
    /// 集成事件
    /// </summary>
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid().ToString("N");
            CreationDate = DateTime.UtcNow;
        }

        [JsonInclude]
        public string Id { get; set; }

        [JsonInclude]
        public DateTime CreationDate { get; set; }
    }
}