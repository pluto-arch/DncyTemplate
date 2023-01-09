using Dncy.Permission;

namespace DncyTemplate.Application.Permission;


/// <summary>
/// 权限定义Provider
/// can read permissions from db or other store device
/// </summary>
public class PermissionDefinitionProvider : IPermissionDefinitionProvider
{
    /// <inheritdoc />
    public void Define(PermissionDefinitionContext context)
    {

        // 产品
        var productGroup = context.AddGroup(ProductPermission.GroupName, "产品管理");
        productGroup.AddPermission(ProductPermission.Product.Default, "产品列表")
            .AddChild(ProductPermission.Product.Detail, "产品详情")
            .AddChild(ProductPermission.Product.Create, "创建产品")
            .AddChild(ProductPermission.Product.Edit, "编辑产品")
            .AddChild(ProductPermission.Product.Delete, "删除产品");

        // 设备
        var deviceGroup = context.AddGroup(DevicesPermission.GroupName, "设备管理");
        deviceGroup.AddPermission(DevicesPermission.Devices.Default, "设备列表")
            .AddChild(DevicesPermission.Devices.Detail, "设备详情")
            .AddChild(DevicesPermission.Devices.Create, "新增设备")
            .AddChild(DevicesPermission.Devices.Edit, "编辑设备")
            .AddChild(DevicesPermission.Devices.Delete, "删除设备");


        // 租户
        var tenantGroup = context.AddGroup(TenantPermission.GroupName, "租户管理");
        tenantGroup.AddPermission(TenantPermission.Tenant.Default, "租户列表")
            .AddChild(TenantPermission.Tenant.Detail, "租户详情");

    }
}


#region permission definitions

public static class DevicesPermission
{
    public const string GroupName = "DeviceManager";

    public static class Devices
    {
        public const string Default = GroupName + ".Devices";
        public const string Detail = Default + ".Detail";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}

public static class ProductPermission
{
    public const string GroupName = "ProductManager";

    public static class Product
    {
        public const string Default = GroupName + ".Products";
        public const string Detail = Default + ".Detail";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}



public static class TenantPermission
{
    public const string GroupName = "TenantManager";

    public static class Tenant
    {
        public const string Default = GroupName + ".Tenants";
        public const string Detail = Default + ".Detail";
    }
}


#endregion