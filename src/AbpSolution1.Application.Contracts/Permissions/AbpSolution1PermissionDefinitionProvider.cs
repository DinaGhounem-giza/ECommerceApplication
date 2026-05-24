using AbpSolution1.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace AbpSolution1.Permissions;

public class AbpSolution1PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpSolution1Permissions.GroupName);
        
        var ordersPermission = myGroup.AddPermission(AbpSolution1Permissions.Orders.Default, L("Permission:Orders"));
        ordersPermission.AddChild(AbpSolution1Permissions.Orders.Create, L("Permission:Orders.Create"));
        ordersPermission.AddChild(AbpSolution1Permissions.Orders.View, L("Permission:Orders.View"));

        var categoriesPermission = myGroup.AddPermission(AbpSolution1Permissions.Categories.Default, L("Permission:Categories"));
        categoriesPermission.AddChild(AbpSolution1Permissions.Categories.Create, L("Permission:Categories.Create"));
        categoriesPermission.AddChild(AbpSolution1Permissions.Categories.View, L("Permission:Categories.View"));
        categoriesPermission.AddChild(AbpSolution1Permissions.Categories.Update, L("Permission:Categories.Update"));
        categoriesPermission.AddChild(AbpSolution1Permissions.Categories.Delete, L("Permission:Categories.Delete"));

        var productsPermission = myGroup.AddPermission(AbpSolution1Permissions.Products.Default, L("Permission:Products"));
        productsPermission.AddChild(AbpSolution1Permissions.Products.Create, L("Permission:Products.Create"));
        productsPermission.AddChild(AbpSolution1Permissions.Products.View, L("Permission:Products.View"));
        productsPermission.AddChild(AbpSolution1Permissions.Products.Update, L("Permission:Products.Update"));
        productsPermission.AddChild(AbpSolution1Permissions.Products.Delete, L("Permission:Products.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSolution1Resource>(name);
    }
}
