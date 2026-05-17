using FitCore.Persistence.Seed;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

public class RoleSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

        string[] roles =
        {
            "SuperAdmin",
            "Admin",
            "Operator",
            "Trainer",
            "Member"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<long>(role));
            }
        }
    }
}
