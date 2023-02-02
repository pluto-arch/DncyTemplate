namespace DncyTemplate.Mvc.Models.Role;

public class RoleDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }


    public List<string> GrantedPermissionNames { get; set; }
}