namespace AbpSolution1.Permissions;

public static class AbpSolution1Permissions
{
    public const string GroupName = "AbpSolution1";


    public static class Orders
    {
        public const string Default = GroupName + ".Orders";
        public const string Create = Default + ".Create";
        public const string View = Default + ".View";
    }
}
