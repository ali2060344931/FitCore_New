using FitCore.Domain.Entities.NutritionProgram.NutritionMeal;
using FitCore.Persistence.Common;
using FitCore.Persistence.Contexts;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitCore.Persistence.Seed
{

    public class MealTypeSeeder : ISeeder
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataBaseContext>();
            var goalTypes = new List<MealType>
            {
                new MealType { Name = "صبحانه" },
                new MealType { Name = "میان‌وعده صبح" },
                new MealType { Name = "ناهار" },
                new MealType { Name = "میان‌وعده عصر" },
                new MealType { Name = "شام" },
                new MealType { Name = "قبل از خواب" },
                new MealType { Name = "قبل از تمرین" },
                new MealType { Name = "بعد از تمرین" }            };

            await context.SeedIfNotExists(
                goalTypes,
                goal => x => x.Name == goal.Name
            );

        }
    }

}
