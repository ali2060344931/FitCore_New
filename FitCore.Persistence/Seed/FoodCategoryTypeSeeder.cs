using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{

    public class FoodCategoryTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
            if (await context.Set<FoodCategoryType>().AnyAsync())
                return;

            context.Set<FoodCategoryType>().AddRange(
                new FoodCategoryType { Name = "پروتئین" },
                new FoodCategoryType {  Name = "کربوهیدرات" },
                new FoodCategoryType {  Name = "چربی" },
                new FoodCategoryType {  Name = "میوه" },
                new FoodCategoryType {  Name = "سبزیجات" },
                new FoodCategoryType {  Name = "نوشیدنی" }
            );

            await context.SaveChangesAsync(default);
        }
    }

}
