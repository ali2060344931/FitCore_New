using FitCore.Common.Roles;
using FitCore.Domain.Entities.Users;
using FitCore.Persistence.Seed;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

/// <summary>
/// ثبت اولیه کاربر سوپر ادمین
/// </summary>
public class SuperAdminSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        var phone = "09111161996";

        var user = await userManager.FindByNameAsync(phone);

        if (user == null)
        {
            var superAdmin = new AppUser
            {
                FullName = "علی اصغر غلامزاده",
                UserName = phone,
                PhoneNumber = phone,
                IsActive = true
            };

            var result = await userManager.CreateAsync(superAdmin, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, UserRoles.SuperAdmin);
            }
        }
    }
}
