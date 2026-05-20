using Volo.Abp.Identity;

namespace AbpSolution1;

public static class AbpSolution1Consts
{
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 200;

    public const string DbTablePrefix = "App";
    public const string? DbSchema = "ECommerce";
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = "1q2w3E*";
}
