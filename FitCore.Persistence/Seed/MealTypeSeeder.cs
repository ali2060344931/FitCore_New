using FitCore.Domain.Entities.NutritionProgram.Food;
using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;
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

    public class MealTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();

            // اگر قبلاً داده‌ها وارد شده‌اند، تکراری وارد نکن
            if (await context.Set<MealType>().AnyAsync())
                return;

            context.Set<MealType>().AddRange(
                new MealType { Name = "صبحانه" },
                new MealType { Name = "میان‌وعده صبح" },
                new MealType { Name = "ناهار" },
                new MealType { Name = "میان‌وعده عصر" },
                new MealType { Name = "شام" },
                new MealType { Name = "قبل از خواب" }
            );

            await context.SaveChangesAsync(default);
        }
    }

}
