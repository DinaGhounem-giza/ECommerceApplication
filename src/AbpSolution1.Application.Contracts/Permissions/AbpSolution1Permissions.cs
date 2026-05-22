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

    public static class Categories
    {
        public const string Default = GroupName + ".Categories";
        public const string Create = Default + ".Create";
        public const string View = Default + ".View";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
    }

    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Create = Default + ".Create";
        public const string View = Default + ".View";
        public const string Delete = Default + ".Delete";
        public const string Update = Default + ".Update";
    }
}
