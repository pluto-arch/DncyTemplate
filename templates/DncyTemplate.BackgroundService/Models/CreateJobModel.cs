namespace DncyTemplate.Job.Models
{
    public class CreateJobModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string GroupName { get; set; }
        public string Interval { get; set; }
        public string Desc { get; set; }
        public string CallUrl { get; set; }
        public string HeaderKey { get; set; }
        public string HeaderValue { get; set; }
    }
}