namespace DncyTemplate.Application.Permission.Models;


public class PermissionGroupDto
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<PermissionDto> Permissions { get; set; }
}


public class PermissionDto
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string ParentName { get; set; }

    public bool IsGrant { get; set; }

    public string[] AllowProviders { get; set; }
}