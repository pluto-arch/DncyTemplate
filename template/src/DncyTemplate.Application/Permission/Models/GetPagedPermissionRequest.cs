using DncyTemplate.Application.Models.Generics;

namespace DncyTemplate.Application.Permission.Models;

public class GetPagedPermissionRequest : PageRequest
{
    public string PermissionName { get; set; }
}