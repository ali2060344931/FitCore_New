using FitCore.Persistence.Seed;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class SeederRunner
{
    public static async Task RunAsync(IServiceProvider serviceProvider)
    {
        var seeders = new List<ISeeder>
        {
            new RoleSeeder(),
            new SettingsSeeder(),
            new ProvinceSeeder(),
            new CitySeeder(),
            new SuperAdminSeeder(),
            new NutritionUnitTypeSeeder(),
            new FoodCategoryTypeSeeder(),
            new MealTypeSeeder(),
            new NutritionProgramTypeSeeder(),
            new GoalTypeSeeder(),
        };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(serviceProvider);
        }
    }
}
