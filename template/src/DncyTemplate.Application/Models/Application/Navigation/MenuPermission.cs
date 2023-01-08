namespace DncyTemplate.Application.Models.Application.Navigation;

public record MenuPermission
{
    public string[] PermissionCode { get; set; }

    public bool RequiredAll { get; set; }

    public bool SkipCheck { get; set; }

    public MenuPermission(string[] permissionCode,bool requiredAll=false)
    {
        PermissionCode = permissionCode;
        RequiredAll = requiredAll;
    }


    public MenuPermission(params string[] permissionCode)
    {
        PermissionCode = permissionCode;
    }


    public MenuPermission(bool skipCheck)
    {
        SkipCheck = skipCheck;
    }

    public MenuPermission()
    {

    }

    public static MenuPermission SkipCheckPermission() => new MenuPermission(true);
}