using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace AbpSolution1.Data
{
    public class IdentityDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IPermissionManager _permissionManager;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;

        public IdentityDataSeedContributor(
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            ILookupNormalizer lookupNormalizer,
            IPermissionManager permissionManager,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _lookupNormalizer = lookupNormalizer;
            _permissionManager = permissionManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var adminRole = await _roleRepository.FindByNormalizedNameAsync(
                _lookupNormalizer.NormalizeName("SystemAdmin"));

            if (adminRole == null)
            {
                adminRole = new IdentityRole(Guid.NewGuid(), "SystemAdmin", context.TenantId)
                {
                    IsDefault = false,
                    IsPublic = true
                };
                await _roleRepository.InsertAsync(adminRole);

                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Orders.View", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Categories.Create", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Categories.View", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Categories.Update", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Categories.Delete", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Products.Create", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Products.View", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Products.Update", true);
                await _permissionManager.SetForRoleAsync("SystemAdmin", "AbpSolution1.Products.Delete", true);
      
            }

            var customerRole = await _roleRepository.FindByNormalizedNameAsync(
                _lookupNormalizer.NormalizeName("Customer"));

            if (customerRole == null)
            {
                customerRole = new IdentityRole(Guid.NewGuid(), "Customer", context.TenantId)
                {
                    IsDefault = true,
                    IsPublic = true
                };
                await _roleRepository.InsertAsync(customerRole);

                await _permissionManager.SetForRoleAsync("Customer", "AbpSolution1.Orders.Create", true);
                await _permissionManager.SetForRoleAsync("Customer", "AbpSolution1.Orders.View", true);
            }

            // Create Users and Assign Roles
            var adminUser = await _userRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("sysAdmin"));

            if (adminUser == null)
            {
                adminUser = new IdentityUser(Guid.NewGuid(), "sysAdmin", "sysAdmin@abpsolution1.com", context.TenantId);
                await _userManager.CreateAsync(adminUser, "Admin@123");
                await _userManager.AddToRoleAsync(adminUser, "SystemAdmin");
            }

            var customerUser = await _userRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("customer"));

            if (customerUser == null)
            {
                customerUser = new IdentityUser(Guid.NewGuid(), "customer", "customer@abpsolution1.com", context.TenantId);
                await _userManager.CreateAsync(customerUser, "Customer@123");
                await _userManager.AddToRoleAsync(customerUser, "Customer");
            }
        }
    }
}