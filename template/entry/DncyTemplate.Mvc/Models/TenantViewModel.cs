namespace DncyTemplate.Mvc.Models;

public class TenantViewModel
{
    public string TenantId { get; set; }

    public string TenantName { get; set; }

    public bool IsAvaliable { get; set; }

    public Dictionary<string, string> ConnectionStrings { get; set; }

}