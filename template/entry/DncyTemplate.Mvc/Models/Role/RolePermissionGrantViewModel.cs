namespace DncyTemplate.Mvc.Models.Role;

public class RolePermissionGrantViewModel
{
    public int Code { get; set; }

    public string Msg { get; set; }

    public List<PermissionTreeViewModel> Data { get; set; }
}