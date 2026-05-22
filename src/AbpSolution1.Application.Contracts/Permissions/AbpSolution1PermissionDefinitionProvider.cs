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
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSolution1Resource>(name);
    }
}
