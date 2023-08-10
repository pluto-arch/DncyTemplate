namespace DncyTemplate.Mvc.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string TraceIdentifier { get; set; }
    }
}