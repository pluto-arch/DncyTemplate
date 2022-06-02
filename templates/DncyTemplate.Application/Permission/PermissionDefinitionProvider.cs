using Dncy.Permission;
using Dncy.Permission.Models;

namespace DncyTemplate.Application.Permission;


/// <summary>
/// 权限定义Provider
/// can read permissions from db or other store device
/// </summary>
public class PermissionDefinitionProvider:IPermissionDefinitionProvider
{
    /// <inheritdoc />
    public void Define(PermissionDefinitionContext context)
    {
        var productGroup = context.AddGroup(ProductPermission.GroupName, "产品管理");
        PermissionDefinition userPermissionManager = productGroup.AddPermission(ProductPermission.Product.Default, "产品列表");
        userPermissionManager.AddChild(ProductPermission.Product.Detail, "产品详情");
        userPermissionManager.AddChild(ProductPermission.Product.Create, "创建产品");
        userPermissionManager.AddChild(ProductPermission.Product.Edit, "编辑产品");
        userPermissionManager.AddChild(ProductPermission.Product.Delete, "删除产品");
    }
}


#region permission definitions
/// <summary>
///     产品权限定义
/// </summary>
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
#endregion