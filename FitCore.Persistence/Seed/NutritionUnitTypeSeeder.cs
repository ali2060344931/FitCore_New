using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Persistence.Contexts;
using FitCore.Persistence.Seed;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Threading.Tasks;

public class NutritionUnitTypeSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<DataBaseContext>();

        // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
        if (await context.Set<NutritionUnitType>().AnyAsync())
            return;

        context.Set<NutritionUnitType>().AddRange(
            new NutritionUnitType {Name = "گرم" },
            new NutritionUnitType {  Name = "عدد / تکه" },
            new NutritionUnitType {  Name = "قاشق" },
            new NutritionUnitType {  Name = "پیمانه / لیوان" },
            new NutritionUnitType {  Name = "برش" }
        );

        await context.SaveChangesAsync(default);
    }
}
